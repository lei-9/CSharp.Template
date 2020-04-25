using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using Xunit;

namespace CSharp.Template.Repositories.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // var sqlConnection = "Server=localhost;Initial Catalog=TemplateDB;User ID=sa;Password=123456;Application Name=zbq;MultipleActiveResultSets=false";
            // var connection = new SqlConnection(sqlConnection);
            // connection.Open();
            // var db = connection.Database;
            var englishWord = "abcdefghijkmn";

            var context = new TempContext();
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "angle "
            };

            //context.Set<User>().AttachRange();

            var tran = context.Database.BeginTransaction();

            context.Set<User>().Attach(user);

            context.Entry(user).State = EntityState.Added;

            context.SaveChanges();

            tran.Commit();
            //context.Database.CommitTransaction();
        }
    }


    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class TempContext : DbContext
    {
        private DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqlConnection = "Server=127.0.0.1;Initial Catalog=TemplateDB;User ID=sa;Password=123456;Application Name=zbq;MultipleActiveResultSets=true";
            // var connection = new SqlConnection(sqlConnection);
            //
            optionsBuilder.UseSqlServer(sqlConnection); //
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");

            //base.OnModelCreating(modelBuilder);
        }
    }
}