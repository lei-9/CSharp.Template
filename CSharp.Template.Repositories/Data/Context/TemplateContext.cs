using System.Reflection;
using CSharp.Framework.Helper;
using CSharp.Template.PersistentObject.Account;
using CSharp.Template.Repositories.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CSharp.Template.Repositories.Data.Context
{
    public class TemplateContext : DbContext
    {
        public TemplateContext(DbContextOptions<TemplateContext> context) : base(context)
        {

        }

        private DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}