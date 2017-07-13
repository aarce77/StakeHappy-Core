using System;
using System.Linq;
using System.Linq.Expressions;

namespace StakHappy.Core.Data
{
    public interface IRepository<T> : IDisposable
    {
        T Save(T entity);
        void Delete(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll();
        T Create();
        int Commit();
    }
}
