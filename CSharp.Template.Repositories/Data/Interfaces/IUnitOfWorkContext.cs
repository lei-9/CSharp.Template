using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CSharp.Template.Repositories.Data.Interfaces
{
    /// <summary>
    /// 工作单元 - 上下文更改操作
    /// </summary>
    public interface IUnitOfWorkContext
    {
        //DbSet<TEntity> Entities { get; }

        Task RegisterNew<TEntity>(TEntity entity);

        Task RegisterNew<TEntity>(IEnumerable<TEntity> entities);

        Task RegisterDelete<TEntity>(TEntity entity);
        
        Task<int> RegisterDelete<TEntity>(IEnumerable<TEntity> entities);

        Task<int> RegisterDelete<TEntity>(Expression<Func<TEntity, bool>> predicate);

        Task RegisterModified<TEntity>(TEntity entity, List<string> fields = null);

        Task<int> RegisterModified<TEntity>(IEnumerable<TEntity> entities, List<string> fields = null);

     
        /// <summary>
        /// 更新查询表达式结果集的字段
        /// </summary>
        /// <param name="whereExpression">where查询表达式</param>
        /// <param name="updateExpression">要更新的字段</param>
        /// <typeparam name="TEntity"></typeparam>
    
        //Task<int> RegisterModified<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression);
    }
}