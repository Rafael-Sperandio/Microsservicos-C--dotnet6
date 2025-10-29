using GeekShoping.IdentityServer.Configurantion;
using GeekShoping.IdentityServer.DbModel;
using GeekShoping.IdentityServer.DbModel.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;


namespace GeekShoping.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // ===== 1. Configuração do banco de dados =====
            var connection = builder.Configuration.GetConnectionString("MySQLConnection");
            builder.Services.AddDbContext<MySQLContext>(options =>
                       options.UseMySql(connection,
                            new MySqlServerVersion(new Version(8, 0, 44))));
            //Indetity Sql Server
            // ===== 2. Configuração do ASP.NET Core Identity =====
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MySQLContext>()
                .AddDefaultTokenProviders();
            // ===== 3. Configuração do Duende IdentityServer =====
            builder.Services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;

                })
                .AddAspNetIdentity<ApplicationUser>() // <-- integra Identity do ASP.NET Core
                .AddInMemoryIdentityResources(IdentityConfigurantion.IdentityResources)
                .AddInMemoryApiScopes(IdentityConfigurantion.ApiScopes)
                .AddInMemoryClients(IdentityConfigurantion.Clients)
                .AddDeveloperSigningCredential(); // apenas para desenvolvimento

            // ===== 4. MVC (Controllers + Views) =====
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ===== 5. Pipeline HTTP =====
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Importante: ordem correta — IdentityServer antes de Authorization
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}