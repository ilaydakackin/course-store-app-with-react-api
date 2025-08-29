using API.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, string>(options)
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new List<Product>
                {
                    new Product
                    {
                        Id=1, Name="IPhone 15", Description="Telefon", ImageUrl = "1.jpg",
                        Price=100000, Stock=100,IsActive=true
                    },
                    new Product
                    {
                        Id=2, Name="IPhone 15", Description="eraregatgtrAW", ImageUrl = "2.jpg",
                        Price=200000, Stock=100,IsActive=true
                    },
                    new Product
                    {
                        Id=3, Name="IPhone 15", Description="reafgagr", ImageUrl = "3.png",
                        Price=300000, Stock=100,IsActive=true
                    },
                    new Product
                    {
                        Id=4, Name="IPhone 15", Description="4tgwerw3erhy", ImageUrl = "4.jpg",
                        Price=400000, Stock=100,IsActive=true
                    },
                    new Product
                    {
                        Id=5, Name="IPhone 15", Description="wre4tfa", ImageUrl = "5.jpg",
                        Price=500000, Stock=100,IsActive=true
                    }
                }
            );
        }

    }
}
