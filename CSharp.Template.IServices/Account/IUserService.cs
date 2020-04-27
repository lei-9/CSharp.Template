using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp.Template.PersistentObject.Account;

namespace CSharp.Template.IServices.Account
{
    public interface IUserService : IBaseService<User>
    {
        Task<List<User>> GetAll();
    }
}