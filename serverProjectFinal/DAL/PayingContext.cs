using Microsoft.EntityFrameworkCore;

using serverProjectFinal.Models;
using System.Collections.Generic;

namespace serverProjectFinal.DAL
{
    public class PayingContext : DbContext
    {
        public PayingContext(DbContextOptions<PayingContext> option) : base(option)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Donor> Donor { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<Lottery> Lottery { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItemList)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

    }
}
