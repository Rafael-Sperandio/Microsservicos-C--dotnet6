
using Microsoft.AspNetCore.Identity;

namespace GeekShoping.IdentityServer.DbModel
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
