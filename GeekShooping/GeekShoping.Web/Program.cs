using GeekShoping.Web.Services.IServices;
using GeekShopping.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace GeekShoping.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddHttpClient<IProductService, ProductService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])
                );

            builder.Services.AddHttpClient<ICartService, CartService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartAPI"])
                );
/*
            builder.Services.AddHttpClient<ICouponService, CouponService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])
                );
*/

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(options => {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:4435";
                    options.ClientId = "geek_shopping";
                    options.ClientSecret = "my_super_secret"; //pode ser alterado para buscar do appsetings ou outro arquivo
                    options.ResponseType = "code";

                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.ClaimActions.MapJsonKey("role", "role", "role");
                    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";


                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("geek_shopping"); // esse é o scope que as APIs exigem
                    options.Scope.Add("offline_access");
                    options.SaveTokens = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //auntenticação antes de autorização
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}