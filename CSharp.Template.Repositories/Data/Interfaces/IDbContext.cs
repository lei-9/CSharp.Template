using System.Threading.Tasks;

namespace CSharp.Template.Repositories.Data.Interfaces
{
    /// <summary>
    /// Context单元 - 保存
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 提交保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}