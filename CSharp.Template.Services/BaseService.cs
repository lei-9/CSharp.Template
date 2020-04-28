using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharp.Template.IRepositories;
using CSharp.Template.IServices;

namespace CSharp.Template.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private Lazy<IBaseRepository<TEntity>> _repository;

        public BaseService(Lazy<IBaseRepository<TEntity>> repository)
        {
            _repository = repository;
        }

        public virtual Task Insert(TEntity entity, bool isSave = true)
        {
            return _repository.Value.Insert(entity, isSave);
        }

        public virtual Task Insert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            return _repository.Value.Insert(entities, isSave);
        }

        public virtual Task Delete(TEntity entity, bool isSave = true)
        {
            return _repository.Value.Delete(entity, isSave);
        }

        public virtual Task Deletes(IEnumerable<TEntity> entities, bool isSave = true)
        {
            return _repository.Value.Deletes(entities, isSave);
        }

        public virtual Task Deletes(Expression<Func<TEntity, bool>> predicate, bool isSave = true)
        {
            return _repository.Value.Deletes(predicate, isSave);
        }

        public virtual Task Update(TEntity entity, List<string> fields = null, bool isSave = true)
        {
            return _repository.Value.Update(entity, fields, isSave);
        }

        public virtual Task Update(IEnumerable<TEntity> entities, List<string> fields = null, bool isSave = true)
        {
            return _repository.Value.Update(entities, fields, isSave);
        }

        public virtual Task Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression, bool isSave = true)
        {
            return _repository.Value.Update(whereExpression, updateExpression, isSave);
        }

        public virtual Task<TEntity> GetByKey(object key)
        {
            return _repository.Value.GetByKey(key);
        }

        public virtual Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Value.FirstOrDefault(predicate);
        }

        public virtual Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null)
        {
            return _repository.Value.GetList(predicate, orderBy, orderByDescending);
        }

        public virtual Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null,
            Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null)
        {
            return _repository.Value.GetList(predicate, skip, take, orderBy, orderByDescending);
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return _repository.Value.GetAll();
        }

        public virtual Task<int> SaveChanges()
        {
            return _repository.Value.SaveChanges();
        }
    }
}