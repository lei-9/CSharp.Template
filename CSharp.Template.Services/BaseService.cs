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
        private IBaseRepository<TEntity> _repository;

        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task Add(TEntity entity)
        {
            return _repository.Add(entity);
        }

        public virtual Task Add(IEnumerable<TEntity> entities)
        {
            return _repository.Add(entities);
        }

        public virtual Task Delete(TEntity entity)
        {
            return _repository.Delete(entity);
        }

        public virtual Task Deletes(IEnumerable<TEntity> entities)
        {
            return _repository.Deletes(entities);
        }

        public virtual Task Deletes(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Deletes(predicate);
        }

        public virtual Task Update(TEntity entity, List<string> fields = null)
        {
            return _repository.Update(entity, fields);
        }

        public virtual Task Update(IEnumerable<TEntity> entities, List<string> fields = null)
        {
            return _repository.Update(entities, fields);
        }

        public virtual Task Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression)
        {
            return _repository.Update(whereExpression, updateExpression);
        }

        public virtual Task<TEntity> GetByKey(object key)
        {
            return _repository.GetByKey(key);
        }

        public virtual Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.FirstOrDefault(predicate);
        }

        public virtual Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null)
        {
            return _repository.GetList(predicate, orderBy, orderByDescending);
        }

        public virtual Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null, Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null)
        {
            return _repository.GetList(predicate, skip, take, orderBy, orderByDescending);
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual Task<int> SaveChanges()
        {
            return _repository.SaveChanges();
        }
    }
}