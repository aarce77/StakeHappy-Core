using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class ClientLogic : LogicBase
    {
        #region Dependencies
        private Data.Persistor.Client _clientPersistor;
        private Data.Persistor.ClientContact _clientContactPersistor;
        #endregion

        #region
        public ClientLogic() {
            _clientPersistor = Dependency.Get<Data.Persistor.Client>();
            _clientContactPersistor = Dependency.Get<Data.Persistor.ClientContact>();
        }
        public ClientLogic(
            Data.Persistor.Client clientPersister, 
            Data.Persistor.ClientContact clientContanctPersistor) {
            _clientPersistor = clientPersister;
            _clientContactPersistor = clientContanctPersistor;
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
            return _clientPersistor.Get(id);
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
            var result = _clientPersistor.Save(client);

            if (!isNew)
            {
                _clientPersistor.Commit();
                return result;
            }

            foreach (var clientContact in client.Contacts)
            {
                clientContact.Client_Id = result.Id;
                _clientContactPersistor.Save(clientContact);   
            }

            _clientPersistor.Commit();
            _clientContactPersistor.Commit();
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

            _clientPersistor.Delete(id);
            _clientPersistor.Commit();
        }

        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <returns></returns>
        public virtual IQueryable<Data.Model.Client> Search(Data.Search.ClientCriteria criteria)
        {
            VaildateCriteria(criteria);
            return _clientPersistor.Search(criteria);
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

            var entity = _clientContactPersistor.Save(contact);
            _clientContactPersistor.Commit();

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

            _clientContactPersistor.Delete(id);
            _clientContactPersistor.Commit();
        }

        public virtual Data.Model.Client GetNewClientObject()
        {
            return _clientPersistor.Create();
        }

        public virtual Data.Model.ClientContact GetNewClientContactObject()
        {
            return _clientContactPersistor.Create();
        }
    }
}
