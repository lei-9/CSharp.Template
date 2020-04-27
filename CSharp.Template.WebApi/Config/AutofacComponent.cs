using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CSharp.Template.WebApi.Config
{
    /// <summary>
    /// autofac 程序初始化注入
    /// </summary>
    public class AutofacComponent
    {
        public static IServiceProvider Register(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将collection中的服务填充到Autofac
            builder.Populate(services);
            //注册InstanceModule组件
            builder.RegisterModule<AutofacConfig>();
            //创建容器
            IContainer container = builder.Build();
            //第三方容器接管Core内置的DI容器
            return new AutofacServiceProvider(container);
        }
    }
}