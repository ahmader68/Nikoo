using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nikoo.Models;

namespace Nikoo.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Action" , DisplayOrder = 1 },
                new Category() { Id = 2, Name = "SciFi" , DisplayOrder = 2 },
                new Category() { Id = 3, Name = "History" , DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product() { 
                    Id = 1,
                    Title = "Product1" ,
                    Description = "Product1 Description",
                    CategoryId = 1,
                    IsActive = true,
                    IsImportant = false,
                    IsSuggested = false,
                    Price = 1000,
                    SellCount = 0,
                    StoreCapacity = 100,
                },
                new Product() { 
                    Id = 2, 
                    Title = "Product2" , 
                    Description = "Product2 Description",
                    CategoryId = 1,
                    IsActive = true,
                    IsImportant = false,
                    IsSuggested = false,
                    Price = 2000,
                    SellCount = 0,
                    StoreCapacity = 200,
                },
                new Product() { 
                    Id = 3, 
                    Title = "Product3" , 
                    Description = "Product3 Description",
                    CategoryId = 2,
                    IsActive = true,
                    IsImportant = false,
                    IsSuggested = false,
                    Price = 3000,
                    SellCount = 0,
                    StoreCapacity = 300,
                },
                new Product() { 
                    Id = 4, 
                    Title = "Product4" , 
                    Description = "Product4 Description",
                    CategoryId = 2,
                    IsActive = true,
                    IsImportant = false,
                    IsSuggested = false,
                    Price = 4000,
                    SellCount = 0,
                    StoreCapacity = 400,
                },
                new Product() { 
                    Id = 5, 
                    Title = "Product3" , 
                    Description = "Product3 Description",
                    CategoryId = 3,
                    IsActive = true,
                    IsImportant = false,
                    IsSuggested = false,
                    Price = 3000,
                    SellCount = 0,
                    StoreCapacity = 300,
                },
                new Product() { 
                    Id = 6, 
                    Title = "Product6" , 
                    Description = "Product6 Description",
                    CategoryId = 3,
                    IsActive = true,
                    IsImportant = false,
                    IsSuggested = false,
                    Price = 6000,
                    SellCount = 0,
                    StoreCapacity = 600,
                }
                );
        }
    }
}
