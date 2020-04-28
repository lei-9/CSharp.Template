using System;
using System.Reflection;
using Autofac;
using CSharp.Template.IRepositories;
using CSharp.Template.IServices;
using CSharp.Template.IServices.Account;
using CSharp.Template.Repositories;
using CSharp.Template.Services;
using CSharp.Template.Services.Account;
using Xunit;

namespace CSharp.Template.WebApi.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            try
            {
                var builder = new ContainerBuilder();
           
                //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
                builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(BaseRepository<>))).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
                //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
                builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(BaseService<>))).Where(a => a.Name.EndsWith("Server")).AsImplementedInterfaces();
                
                builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());  
                
                using (var container = builder.Build())
                {
                    using (var scope = container.BeginLifetimeScope("request"))
                    {
                        var userService = scope.Resolve<IUserService>();
                        //var userService = container.Resolve<UserService>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}