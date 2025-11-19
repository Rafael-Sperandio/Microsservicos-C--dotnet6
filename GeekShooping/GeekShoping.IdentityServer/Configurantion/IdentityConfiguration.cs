using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShoping.IdentityServer.Configurantion
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        // Recursos de identidade padrão
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        // Escopos das APIs
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("geek_shopping", "Geek Shopping Server"),
                new ApiScope("read", "Read data."),
                new ApiScope("write", "Write data."),
                new ApiScope("delete", "Delete data.")
            };

        // ✅ Aqui está o que faltava:
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("geek_shopping", "Geek Shopping API")
                {
                    Scopes = { "geek_shopping", "read", "write", "delete" },
                    ApiSecrets = { new Secret("my_super_secret".Sha256()) }
                }
            };

        // Clientes (aplicações)
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // Cliente interno (para chamadas automáticas de serviço)
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "read", "write", "profile", "geek_shopping" }
                },
                
                // Cliente MVC (frontend)
                new Client
                {
                    ClientId = "geek_shopping",
                    ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:4430/signin-oidc" }, // ⚠️ troque a porta se for diferente
                    PostLogoutRedirectUris = { "https://localhost:4430/signout-callback-oidc" },
                    AllowedCorsOrigins = { "https://localhost:4430" }, // ✅ adicione isto
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,

                        // 👇 escopos customizados que sua WebApp pede
                        "geek_shopping",
                        "read",
                        "write",
                        "offline_access"
                    },

                    AllowOfflineAccess = true, // necessário para refresh tokens
                    RequirePkce = true


                },
            };

    }
}
