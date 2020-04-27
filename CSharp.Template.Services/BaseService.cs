using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharp.Template.IRepositories;
using CSharp.Template.IServices;

namespace CSharp.Template.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        private Lazy<IBaseRepository<TEntity>> _repository;

        public BaseService(Lazy<IBaseRepository<TEntity>> repository)
        {
            _repository = repository;
        }

        public virtual Task Add(TEntity entity)
        {
            return _repository.Value.Add(entity);
        }

        public virtual Task Add(IEnumerable<TEntity> entities)
        {
            return _repository.Value.Add(entities);
        }

        public virtual Task Delete(TEntity entity)
        {
            return _repository.Value.Delete(entity);
        }

        public virtual Task Deletes(IEnumerable<TEntity> entities)
        {
            return _repository.Value.Deletes(entities);
        }

        public virtual Task Deletes(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Value.Deletes(predicate);
        }

        public virtual Task Update(TEntity entity, List<string> fields = null)
        {
            return _repository.Value.Update(entity, fields);
        }

        public virtual Task Update(IEnumerable<TEntity> entities, List<string> fields = null)
        {
            return _repository.Value.Update(entities, fields);
        }

        public virtual Task Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression)
        {
            return _repository.Value.Update(whereExpression, updateExpression);
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

        public virtual Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null, Expression<Func<TEntity, object>> orderBy = null,
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