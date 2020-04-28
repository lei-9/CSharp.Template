using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CSharp.Template.IRepositories;
using CSharp.Template.IServices;
using CSharp.Template.IServices.Account;
using CSharp.Template.Repositories;
using CSharp.Template.Repositories.Data.Context;
using CSharp.Template.Services;
using CSharp.Template.Services.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CSharp.Template.WebApi.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // try
            // {
            //     var builder = new ContainerBuilder();
            //
            //   
            //     builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            //     var service = Assembly.Load("CSharp.Template.Services");
            //     builder.RegisterAssemblyTypes(service).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            //     builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            //     
            //     //数据库上下文注入
            //     builder.Register(c =>
            //     {
            //         var config = c.Resolve<IConfiguration>();
            //
            //         var opt = new DbContextOptionsBuilder<TemplateContext>();
            //         //获取配置的连接串
            //         opt.UseSqlServer(config.GetSection("DefaultConnectionString").Value);
            //
            //         return new TemplateContext(opt.Options);
            //     }).AsSelf().InstancePerLifetimeScope();
            //
            //     using (var container = builder.Build())
            //     {
            //         var userService = container.Resolve<IUserService>();
            //         var list =  userService.GetAll().Result;
            //         var a = 1;
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }
        }
    }
}