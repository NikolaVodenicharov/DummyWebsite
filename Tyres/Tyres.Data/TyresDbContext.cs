using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tyres.Data.Models;
using Tyres.Data.Models.Orders;
using Tyres.Data.Models.Products;

namespace Tyres.Data
{
    public class TyresDbContext : IdentityDbContext<User>
    {
        public TyresDbContext(DbContextOptions<TyresDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tyre> Tyres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            builder
                .Entity<Item>()
                .HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId);
        }
    }
}
