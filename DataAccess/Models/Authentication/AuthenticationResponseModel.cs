using Org.BouncyCastle.Bcpg;

namespace DataAccess.Models.Authentication;

public class AuthenticationResponseModel : ApiResponse
{
    public string Type { get; set; }
    public string Role { get; set; }
}