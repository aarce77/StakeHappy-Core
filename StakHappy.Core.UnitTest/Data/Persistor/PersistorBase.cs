using System;

namespace StakHappy.Core.UnitTest.Data.Persistor
{
    public abstract class PersistorBase : IDisposable
    {
        private readonly System.Transactions.TransactionScope _transScope;

        protected PersistorBase()
        {
            _transScope = new System.Transactions.TransactionScope();
        }

        public void Dispose()
        {
            _transScope.Dispose();
        }
    }
}
