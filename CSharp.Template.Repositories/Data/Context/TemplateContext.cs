using CSharp.Framework.Helper;
using Microsoft.EntityFrameworkCore;

namespace CSharp.Template.Repositories.Data.Context
{
    public class TemplateContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConfigHelper.Get("DefaultConnectionString"));
            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}