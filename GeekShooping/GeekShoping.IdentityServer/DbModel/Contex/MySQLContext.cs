using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShoping.IdentityServer.DbModel.Context
{
    public class MySQLContext : IdentityDbContext<ApplicationUser>
    {
        public MySQLContext() {}
        public MySQLContext(DbContextOptions<MySQLContext> options): base(options)
        {

        }
        //public DbSet<Product> Products { get; set;}
 /*        
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Name = "Camiseta No Internet",
                Price = new decimal(69.9),
                Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.<br/>The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English.<br/>Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy.",
                ImageURL = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/2_no_internet.jpg?raw=true",
                CategoryName = "T-shirt"
            });

        }*/


    }
}