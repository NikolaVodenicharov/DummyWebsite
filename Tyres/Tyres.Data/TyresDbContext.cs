using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tyres.Data.Models;
using Tyres.Data.Models.Orders;
using Tyres.Products.Data.Models;

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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<User>()
                .HasOne<Cart>(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);

            builder
                .Entity<Cart>()
                .HasKey(c => c.Id);

            builder
                .Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);


            builder
                .Entity<Item>()
                .HasKey(c => c.Id);

            builder
                .Entity<Item>()
                .HasOne(i => i.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CartId);

            builder
                .Entity<Item>()
                .HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId);
        }
    }
}
