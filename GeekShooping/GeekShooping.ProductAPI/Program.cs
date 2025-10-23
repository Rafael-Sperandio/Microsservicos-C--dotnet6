using GeekShooping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GeekShooping.ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //("MySQLConection:MySQLConnectionString"
            //builder.Configuration.GetConnectionString("DefaultConnection");


            var connection = builder.Configuration.GetConnectionString("MySQLConnection");
            builder.Services.AddDbContext<MySQLContext>(options =>
                       options.UseMySql(connection,
                            new MySqlServerVersion(new Version(8,0,44))));
            //UseSqlServer(connection)
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.ProductAPI", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
