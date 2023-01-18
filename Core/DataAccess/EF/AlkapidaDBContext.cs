using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EF
{
    public class Furkan_TaskDBContext : DbContext
    {
        public Furkan_TaskDBContext(DbContextOptions<Furkan_TaskDBContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<UserPermission> Users_Permissions { get; set; }
        public DbSet<New> New { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
