using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSharp.Template.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        //Lazy<IQueryable<T>> Entities { get; }

        // #region add
        //
        // Task Add(TEntity entity);
        //
        // Task Add(IEnumerable<TEntity> entities);
        //
        // #endregion
        //
        // #region delete
        //
        // Task Delete(TEntity entity);
        //
        // Task Deletes(IEnumerable<TEntity> entities);
        //
        // #endregion
        //
        // #region update
        //
        // Task Update(TEntity entity, List<string> fields = null);
        // Task Update(IEnumerable<TEntity> entities, List<string> fields = null);
        //
        // #endregion
        //
        // #region query
        //
        // TEntity GetByKey(object key);
        //
        // TEntity FirstOrDefault(Expression<Func<TEntity, bool>> whereExpression);
        // IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderBy = null);
        //
        // #endregion
    }
}