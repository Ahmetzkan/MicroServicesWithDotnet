using System.Text.Json.Serialization;

namespace keycloak.web.Services
{
    public class RoleAsClaim
    {
        [JsonPropertyName("roles")] public List<string>? Roles { get; set; }
    }
}