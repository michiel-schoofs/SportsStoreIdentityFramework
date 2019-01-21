using IdentityFrameworkSportsstore.Models.Domain;
using IdentityFrameworkSportsstore.Data.Mappers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityFrameworkSportsstore.Data {
    public class ApplicationDbContext : IdentityDbContext {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration<Category>(new CategoryConfiguration())
                .ApplyConfiguration<Customer>(new CustomerConfiguration())
                .ApplyConfiguration<City>(new CityConfiguration())
                .ApplyConfiguration<Order>(new OrderConfiguration())
                .ApplyConfiguration<OrderLine>(new OrderLineConfiguration())
                .ApplyConfiguration<Product>(new ProductConfiguration());
        }
    }
}
