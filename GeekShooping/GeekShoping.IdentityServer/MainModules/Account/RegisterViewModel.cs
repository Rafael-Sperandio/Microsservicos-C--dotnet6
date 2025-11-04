using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekShopping.IdentityServer.MainModule.Account
{
    public class RegisterViewModel
    {
        [ValidateNever]
        public IEnumerable<string>? Roles { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public string RoleName { get; set; }

        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        [ValidateNever]
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        [ValidateNever]
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;

        [ValidateNever]
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
}
