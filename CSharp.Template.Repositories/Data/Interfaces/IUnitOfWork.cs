using System.Threading.Tasks;

namespace CSharp.Template.Repositories.Data.Interfaces
{
    /// <summary>
    /// 工作单元 - 持久化事务管理
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 事务状态
        /// true 已提交
        /// false 未提交
        /// </summary>
        bool Committed { get; }
        
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        Task<int> Commit();
        
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        Task Rollback();
    }
}