using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShoping.IdentityServer.Configurantion
{
    public static class IdentityConfigurantion
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("geek_shopping","Geek Shooping Server"),
                new ApiScope(name:"read","Read data."),
                new ApiScope(name:"write","Write data."),
                new ApiScope(name:"delete","Delete data."),

            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                { 
                    ClientId = "client",
                    ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"read","write","profile"}

                },
                new Client
                {
                    ClientId = "geek_shopping",
                    ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:4430/singin-oidc"},
                    //RedirectUris = {"http://localhost:34198/singin-oidc"},
                    //geekShopping.web launchsetings applicationUrl
                    PostLogoutRedirectUris = {"https://localhost:4430/singout-callback-oidc" },
                    //PostLogoutRedirectUris = {"https://localhost:34198/singout-callback-oidc" },
                    AllowedScopes =  new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        "geek_shopping"
                    }

                },
            };

    }
}
