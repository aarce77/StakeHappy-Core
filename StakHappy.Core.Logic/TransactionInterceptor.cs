using System;
using Ninject.Extensions.Interception;

namespace StakHappy.Core.Logic
{
    internal class TransactionInterceptor : IInterceptor
    {
        private System.Transactions.TransactionScope _transScope;

        public void Intercept(IInvocation invocation)
        {
            try
            {
                BeginTransaction();
                invocation.Proceed();

                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        internal void BeginTransaction()
        {
            _transScope = new System.Transactions.TransactionScope();
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        internal void CommitTransaction()
        {
            if (_transScope != null)
            {
                _transScope.Complete();
                _transScope.Dispose();
            }
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        internal void RollbackTransaction()
        {
            if (_transScope != null)
                _transScope.Dispose();
        }
    }
}
