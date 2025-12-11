using GeekShopping.CartAPI.Repository;
using GeekShopping.OrderAPI.MessageConsumer;
using GeekShopping.OrderAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GeekShooping.OrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //inicio do copia cola

            var connection = builder.Configuration.GetConnectionString("MySQLConnection");
            builder.Services.AddDbContext<MySQLContext>(options =>
                       options.UseMySql(connection,
                            new MySqlServerVersion(new Version(8, 0, 44))));


            //não são utilizados mappers
            /*            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
                        builder.Services.AddSingleton(mapper);
                        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());*/

            // Add services to the container.
            var dbBuild = new DbContextOptionsBuilder<MySQLContext>();
            dbBuild.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 44)));
            builder.Services.AddSingleton(new OrderRepository(dbBuild.Options));//ao invés de adicionar um repository como scopped
                                                                                //builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            
            builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();



            builder.Services.AddControllers();


            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:4435/";//TODO possivel deixar no launch Settings
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();//requisita usuario authenticado
                    policy.RequireClaim("scope", "geek_shopping");
                });
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //rever swagger

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.OrderAPI", Version = "v1" });
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token!",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In= ParameterLocation.Header
                        },
                        new List<string> ()
                    }
                 });
            });

            //fim copia cola

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
