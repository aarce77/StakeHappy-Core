using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class ClientLogic : LogicBase
    {
        #region Dependencies
        private Data.Persistor.Client _clientPersistor;
        internal Data.Persistor.Client ClientPersistor
        {
            get { return Dependency.Get(_clientPersistor); }
            set { _clientPersistor = value; }
        }

        private Data.Persistor.ClientContact _clientContactPersistor;
        internal Data.Persistor.ClientContact ClientContanctPersistor
        {
            get { return Dependency.Get(_clientContactPersistor); }
            set { _clientContactPersistor = value; }
        }
        #endregion

        /// <summary>
        /// Reads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual Data.Model.Client Get(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("client id cannot be empty");
            return ClientPersistor.Get(id);
        }

        /// <summary>
        /// Saves the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        [TransactionInterceptor]
        public virtual Data.Model.Client Save(Data.Model.Client client)
        {
            if (client.User_Id == Guid.Empty)
                throw new ArgumentException("User id most be specified to save a client");

            var isNew = client.Id == Guid.Empty;
            var result = ClientPersistor.Save(client);

            if (!isNew)
            {
                ClientPersistor.Commit();
                return result;
            }

            foreach (var clientContact in client.Contacts)
            {
                clientContact.Client_Id = result.Id;
                ClientContanctPersistor.Save(clientContact);   
            }

            ClientPersistor.Commit();
            ClientContanctPersistor.Commit();
            return result;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentException">client id cannot be empty</exception>
        [TransactionInterceptor]
        public virtual void Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("client id cannot be empty");

            ClientPersistor.Delete(id);
            ClientPersistor.Commit();
        }

        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <returns></returns>
        public virtual IQueryable<Data.Model.Client> Search(Data.Search.ClientCriteria criteria)
        {
            VaildateCriteria(criteria);
            return ClientPersistor.Search(criteria);
        }

        /// <summary>
        /// Gets the contants.
        /// </summary>
        /// <param name="contact">The client contact.</param>
        /// <returns></returns>
        [TransactionInterceptor]
        public virtual Data.Model.ClientContact SaveContact(Data.Model.ClientContact contact)
        {
            if(contact.Client_Id == Guid.Empty)
                throw new ArgumentException("client id cannot be empty");

            var entity = ClientContanctPersistor.Save(contact);
            ClientContanctPersistor.Commit();

            return entity;
        }

        /// <summary>
        /// Deletes the contact.
        /// </summary>
        /// <param name="id">The client contact identifier.</param>
        [TransactionInterceptor]
        public virtual void DeleteContact(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("client contact id cannot be empty");

            ClientContanctPersistor.Delete(id);
            ClientContanctPersistor.Commit();
        }

        public virtual Data.Model.Client GetNewClientObject()
        {
            return ClientPersistor.Create();
        }

        public virtual Data.Model.ClientContact GetNewClientContactObject()
        {
            return ClientContanctPersistor.Create();
        }
    }
}
