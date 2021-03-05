using CommonWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<Users> users { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Rights> Rights { get; set; }
        public DbSet<RoleRights> RoleRights { get; set; }
        public DbSet<Categories> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleRights>()
                .HasKey(c => new { c.RoleID, c.RightID });
        }
    }
}
