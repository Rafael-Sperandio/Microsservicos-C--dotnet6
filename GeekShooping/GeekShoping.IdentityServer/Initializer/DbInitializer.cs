using GeekShoping.IdentityServer.Configurantion;
using GeekShoping.IdentityServer.DbModel;
using GeekShoping.IdentityServer.DbModel.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShooping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context, 
            UserManager<ApplicationUser> user, 
            RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            var temAdm = _role.FindByIdAsync(IdentityConfiguration.Admin).GetAwaiter().GetResult() == null 
                && _role.FindByNameAsync(IdentityConfiguration.Admin).GetAwaiter().GetResult() == null;

            if (temAdm)
            {
                //roles
                _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin))
                    .GetAwaiter().GetResult();
                //user
                ApplicationUser admin = new ApplicationUser() { 
                    UserName="rafael-admin",
                    Email= "rafaelsperandio2001@gmail.com",
                    EmailConfirmed= true,
                    PhoneNumber = "55 (34) 12345-6789",
                    FirstName = "Rafael",
                    LastName = "Admin",
                };
                _user.CreateAsync(admin,"Rafael123$").GetAwaiter().GetResult();  
                _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();
                var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name,$"{admin.FirstName} {admin.LastName}"),
                    new Claim(JwtClaimTypes.GivenName,$"{admin.FirstName}"),
                    new Claim(JwtClaimTypes.FamilyName,$"{admin.LastName}"),
                    new Claim(JwtClaimTypes.Role,IdentityConfiguration.Admin),
                }).Result;
            }
            var temClient = _role.FindByIdAsync(IdentityConfiguration.Client).GetAwaiter().GetResult() == null
                         && _role.FindByNameAsync(IdentityConfiguration.Client).GetAwaiter().GetResult() == null;
            if (temClient)
            {
                _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client))
                    .GetAwaiter().GetResult();
                //user
                ApplicationUser client = new ApplicationUser()
                {
                    UserName = "rafael-client",
                    Email = "rafaelsperandio2001@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "55 (34) 12345-6789",
                    FirstName = "Rafael",
                    LastName = "Client",
                };
                _user.CreateAsync(client, "Rafael123$").GetAwaiter().GetResult();
                _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();
                //claims
                var clientClaims = _user.AddClaimsAsync(client, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name,$"{client.FirstName} {client.LastName}"),
                    new Claim(JwtClaimTypes.GivenName,$"{client.FirstName}"),
                    new Claim(JwtClaimTypes.FamilyName,$"{client.LastName}"),
                    new Claim(JwtClaimTypes.Role,IdentityConfiguration.Client),
                }).Result;
            }
        }
    }
}
