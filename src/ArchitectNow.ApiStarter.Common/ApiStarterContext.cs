using System.Reflection;
using ArchitectNow.ApiStarter.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchitectNow.ApiStarter.Common
{
    public class ApiStarterContext : DbContext
    {
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<User> User { get; set; }
        
        public ApiStarterContext(DbContextOptions<ApiStarterContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}