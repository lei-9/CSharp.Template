using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSharp.Template.IServices
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Add(TEntity entity);

        /// <summary>
        /// 新增集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task Add(IEnumerable<TEntity> entities);

        #endregion

        #region 删除

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Delete(TEntity entity);

        /// <summary>
        /// 删除实体集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task Deletes(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除符合条件的实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task Deletes(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 更新

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fields">要更新的字段，null= 全量更新</param>
        /// <returns></returns>
        Task Update(TEntity entity, List<string> fields = null);

        /// <summary>
        /// 更新实体对象集合
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="fields">要更新的字段，null= 全量更新</param>
        /// <returns></returns>
        Task Update(IEnumerable<TEntity> entities, List<string> fields = null);

        /// <summary>
        /// 更新查询出来的实体对象集合 
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression);

        #endregion

        #region 查询

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TEntity> GetByKey(object key);


        /// <summary>
        /// 根据查询条件获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询集合对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDescending"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null);

        /// <summary>
        /// 查询集合对象 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDescending"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null, Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAll();

        #endregion

        /// <summary>
        /// 提交持久化
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChanges();
    }
}