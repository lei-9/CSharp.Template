using System;
using System.Reflection;
using System.Security.Claims;
using Autofac;
using CSharp.StackExchangeRedis;
using CSharp.StackExchangeRedis.Core;
using CSharp.Template.IRepositories;
using CSharp.Template.Repositories;
using CSharp.Template.Repositories.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;

namespace CSharp.Template.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注入上下文配置
            //services.AddDbContextPool<TemplateContext>(option => { option.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")); });

            services.AddControllers();
            //.AddControllersAsServices();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "my api", Version = "v1"}); });
        }

        #region autofac 配置

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注入redis
            builder.RegisterType(typeof(RedisCached)).As(typeof(IRedisCached));

            //数据库上下文注入
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<TemplateContext>();
                //获取配置的连接串
                opt.UseMySQL(config.GetSection("DefaultConnectionString").Value);

                //
                var loggerFactory = c.Resolve<ILoggerFactory>();

                opt.UseLoggerFactory(loggerFactory);

                return new TemplateContext(opt.Options);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            builder.RegisterAssemblyTypes(Assembly.Load("CSharp.Template.Services")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
        }

        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //todo
            #region 错误处理 

            app.UseExceptionHandler(exConfig =>
            {
                exConfig.Run(async context =>
                {
                    await context.Response.WriteAsync("error");
                    //context.User = ClaimsPrincipal.Current;
                });
            });

            #endregion


            #region swagger 配置

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}