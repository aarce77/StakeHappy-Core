using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class ClientLogic : LogicBase<Data.Model.Client>
    {
        #region Dependencies
        private Data.Persistor.ClientContact ClientContactPersistor;
        #endregion

        #region Constructor
        public ClientLogic() {
            Persistor = Dependency.Get<Data.Persistor.Client>();
            ClientContactPersistor = Dependency.Get<Data.Persistor.ClientContact>();
        }
        public ClientLogic(
            Data.Persistor.Client clientPersister, 
            Data.Persistor.ClientContact clientContanctPersistor) {
            Persistor = clientPersister;
            ClientContactPersistor = clientContanctPersistor;
        }
        #endregion

        /// <summary>
        /// Saves the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        [TransactionInterceptor]
        public override Data.Model.Client Save(Data.Model.Client client)
        {
            if (client.User_Id == Guid.Empty)
                throw new ArgumentException("User id most be specified to save a client");

            var isNew = client.Id == Guid.Empty;
            var result = Persistor.Save(client);

            if (!isNew)
            {
                Persistor.Commit();
                return result;
            }

            foreach (var clientContact in client.Contacts)
            {
                clientContact.Client_Id = result.Id;
                ClientContactPersistor.Save(clientContact);   
            }

            Persistor.Commit();
            ClientContactPersistor.Commit();
            return result;
        }

        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <returns></returns>
        public virtual IQueryable<Data.Model.Client> Search(Data.Search.ClientCriteria criteria)
        {
            VaildateCriteria(criteria);
            return (Persistor as Data.Persistor.Client).Search(criteria);
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

            var entity = ClientContactPersistor.Save(contact);
            ClientContactPersistor.Commit();

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

            ClientContactPersistor.Delete(id);
            ClientContactPersistor.Commit();
        }

        public virtual Data.Model.ClientContact GetNewClientContactModel()
        {
            return ClientContactPersistor.Create();
        }
    }
}
