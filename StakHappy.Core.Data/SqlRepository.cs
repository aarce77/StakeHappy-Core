using System;
using System.Linq;
using System.Data.Entity;

namespace StakHappy.Core.Data
{
    public class SqlRepository<T> : IRepository<T> where T : class, Model.IEntity, new()
    {
        private readonly DbContext _dbCtx;
        private readonly DbSet<T> _set;

        public SqlRepository(DbContext dbCtx)
        {
            _dbCtx = dbCtx;
            _set = _dbCtx.Set<T>();
        }

        public T Save(T entity)
        {
            if (entity.Id != Guid.Empty)
            {
                _set.Attach(entity);
                _dbCtx.Entry(entity).State = EntityState.Modified;
                return entity;
            }

            entity.Id = Guid.NewGuid();
            return _set.Add(entity);
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public IQueryable<T> FindAll()
        {
            return _set;
        }

        public T Create()
        {
            return new T();
        }

        public int Commit()
        {
            return _dbCtx.SaveChanges();
        }

        public void Dispose()
        {
            _dbCtx.Dispose();
        }
    }
}
