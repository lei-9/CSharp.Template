using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using CSharp.Template.IRepositories;
using CSharp.Template.IServices;
using CSharp.Template.IServices.Account;
using CSharp.Template.Repositories;
using CSharp.Template.Repositories.Data.Context;
using CSharp.Template.Services;
using CSharp.Template.Services.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CSharp.Template.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注入上下文配置
            services.AddDbContextPool<TemplateContext>(option => { option.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")); });

            services.AddControllers();
            //.AddControllersAsServices();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "my api", Version = "v1"}); });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));

            // var service = Assembly.Load("CSharp.Template.Services");
            // var repository = Assembly.Load("CSharp.Template.Repositories");
            // builder.RegisterAssemblyTypes(service, repository).AsImplementedInterfaces();

            builder.RegisterType(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            builder.RegisterType(typeof(BaseService<>)).As(typeof(IBaseService<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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