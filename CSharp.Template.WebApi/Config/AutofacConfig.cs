using System.Reflection;
using Autofac;
using CSharp.Template.IServices;
using CSharp.Template.Repositories;
using CSharp.Template.Services;

namespace CSharp.Template.WebApi.Config
{
    /// <summary>
    /// autofac 注入配置
    /// </summary>
    public class AutofacConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注入Service
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(BaseService<>))).Where(a=>a.Name.EndsWith("Service")).AsImplementedInterfaces();

            //注入Repository
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(BaseRepository<>))).Where(a=>a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            
            //builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            
            //builder.RegisterDynamicProxy();
            //base.Load(builder);
        }
    }
}