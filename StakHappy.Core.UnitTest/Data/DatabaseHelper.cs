using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace StakHappy.Core.UnitTest.Data
{
    internal static class DatabaseHelper
    {
        #region Members
        private static string _dbConnectionString;

        #endregion

        #region Constructor
        static DatabaseHelper()
        {
            _dbConnectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        }
        #endregion

        #region Methods

        #region GetData
        public static List<T> GetData<T>(string sql)
        {
            return GetData<T>(sql, null);
        }

        public static List<T> GetData<T>(string sql, IDataReader dataReader)
        {
            if(dataReader == null)
                dataReader = GetDataReader(sql);

            var list = new List<T>();
            Type type = typeof(T);

            Debug.WriteLine("==============================================================");
            Debug.WriteLine("Namespace: " + type.Namespace);
            Debug.WriteLine("Name: " + type.Name);

            while (dataReader.Read())
            {
                // the magic that'll create a new instance of the generic type;
                T o = (T)Activator.CreateInstance(type);
                bool columnBound = false;

                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo p in properties)
                {
                    try // there is no guarantee that type will have a column name match 
                    {
                        Debug.WriteLine("Property Name: " + p.Name);

                        string columnName = string.Empty;
                        bool ignore = false;

                        if (p.GetCustomAttributes(true) != null)
                        {
                            List<CustomAttributeData> caList = p.GetCustomAttributesData().ToList();
                            foreach (CustomAttributeData ca in caList)
                            {
                                if (ca.AttributeType.ToString().Contains("LiteCart.Foundation.Model.IgnoreProperty"))
                                    ignore = (bool)ca.ConstructorArguments[0].Value;
                                if (ca.ToString().Contains("LiteCart.Foundation.Model.DatabaseColumnName"))
                                    columnName = ca.ConstructorArguments[0].Value.ToString();
                            }
                        }

                        if (!ignore)
                        {
                            var ordinal = dataReader.GetOrdinal(!String.IsNullOrEmpty(columnName) ? columnName : p.Name);

                            p.SetValue(o, dataReader[ordinal], null);
                            columnBound = true;
                        }
                    }
                    catch (Exception e) { Debug.WriteLine("DatabaseHepler Error: " + e.Message); }
                }

                if (columnBound) // least one column has to have beed bound, or else don't add an empty object to the collection
                  list.Add(o);
            }
            
            return list;
        }
        #endregion

        #region GetDataReader
        public static IDataReader GetDataReader(string sql)
        {
            return GetDataReader(sql, null, CommandType.Text, true);
        }

        public static IDataReader GetDataReader(string sql, CommandType commandType)
        {
            return GetDataReader(sql, null, commandType, true);
        }

        public static IDataReader GetDataReader(string sql, Dictionary<String, IDataParameter> parameters, CommandType commandType, bool clearParameters)
        {
            IDataReader dr1 = null;
            SqlCommand cmd = GetCommandObject(sql, commandType);
            SetParameters(cmd, parameters);

            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                if ((parameters!=null) && clearParameters)
                    parameters.Clear();

                // dispose of resources
                if (dr1 != null)
                {
                    dr1.Close();
                    dr1.Dispose();
                }
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();

                cmd.Dispose();

                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw;
            }

            return dr1;
        }
        #endregion

        #region ExecuteQuery
        public static bool ExecuteQuery(string sql)
        {
            return ExecuteQuery(sql, null, CommandType.Text, true);
        }

        public static bool ExecuteQuery(string sql, CommandType commandType)
        {
            return ExecuteQuery(sql, null, commandType, true);
        }

        public static bool ExecuteQuery(string sql, Dictionary<String, IDataParameter> parameters, CommandType commandType, bool clearParameters)
        {
            bool flag;

            SqlCommand cmd = GetCommandObject(sql, commandType);
            SetParameters(cmd, parameters);

            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                flag = Convert.ToBoolean(cmd.ExecuteNonQuery());

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                if (parameters != null && clearParameters)
                    parameters.Clear();

                cmd.Connection.Dispose();
            }
            catch (Exception ex)
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();

                if (parameters != null)
                    parameters.Clear();

                cmd.Dispose();

                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
                cmd.Dispose();
            }

            return flag;
        }
        #endregion

        #region GetScalar
        public static object GetScalar(string sql)
        {
            return GetScalar(sql, CommandType.Text);
        }

        public static object GetScalar(string sql, CommandType commandType)
        {
            return GetScalar(sql, null, CommandType.Text, true);
        }

        public static object GetScalar(string sql, Dictionary<String, IDataParameter> parameters, CommandType commandType, bool clearParameters)
        {
            object retval;

            SqlCommand cmd = GetCommandObject(sql, commandType);
            SetParameters(cmd, parameters);

            try
            {
                if(cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                retval = cmd.ExecuteScalar();

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                if (parameters != null && clearParameters)
                    parameters.Clear();

                cmd.Connection.Dispose();
            }
            catch (Exception ex)
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                
                if(parameters != null)
                    parameters.Clear();

                cmd.Dispose();

                if (ex.InnerException != null)
                    throw ex.InnerException;
                
                throw;
            }

            return retval;
        }
        #endregion

        private static void SetParameters(SqlCommand cmd, Dictionary<String, IDataParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<String, IDataParameter> p in parameters)
                    cmd.Parameters.Add(p.Value);
            }
        }

        private static SqlCommand GetCommandObject(string sql, CommandType commandType)
        {
            return new SqlCommand(sql, GetConnectionObject()) {CommandType = commandType};
        }

        public static SqlConnection GetConnectionObject(string name = "")
        {
            SqlConnection dbConn;
            if(String.IsNullOrEmpty(_dbConnectionString))
                _dbConnectionString = String.IsNullOrEmpty(name) ? ConfigurationManager.AppSettings["dbConnection"] : ConfigurationManager.AppSettings[name];

            if (!String.IsNullOrEmpty(_dbConnectionString))
            {
                dbConn = new SqlConnection(_dbConnectionString);
                if (dbConn.State != ConnectionState.Open)
                    dbConn.Open();
            }
            else
                throw new Exception("Database connection string was not set");

            return dbConn;
        }
        #endregion
    }
}