using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSharp.Template.Repositories.Data.Interfaces
{
    /// <summary>
    /// 工作单元 - 上下文更改持久操作
    /// </summary>
    public interface IUnitOfWorkContext : IUnitOfWork
    {
        /// <summary>
        /// 插入新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task RegisterNew<TEntity>(TEntity entity);

        /// <summary>
        /// 插入新实体集合
        /// </summary>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task RegisterNew<TEntity>(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task RegisterDelete<TEntity>(TEntity entity);
        
        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<int> RegisterDelete<TEntity>(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除表达式查询结果
        /// </summary>
        /// <param name="predicate"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<int> RegisterDelete<TEntity>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fields"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task RegisterModified<TEntity>(TEntity entity, List<string> fields = null);

        /// <summary>
        /// 更新集合
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="fields"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<int> RegisterModified<TEntity>(IEnumerable<TEntity> entities, List<string> fields = null);

     
        /// <summary>
        /// 更新查询表达式结果集的字段
        /// </summary>
        /// <param name="whereExpression">where查询表达式</param>
        /// <param name="updateExpression">要更新的字段</param>
        /// <typeparam name="TEntity"></typeparam>
    
        Task<int> RegisterModified<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression);
    }
}