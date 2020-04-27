using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp.Template.IRepositories;
using CSharp.Template.IServices.Account;
using CSharp.Template.PersistentObject.Account;

namespace CSharp.Template.Services.Account
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(Lazy<IBaseRepository<User>> userRepository) : base(userRepository)
        {
        }

        public async Task<List<User>> GetAll()
        {
            return await base.GetAll();
        }
    }
}