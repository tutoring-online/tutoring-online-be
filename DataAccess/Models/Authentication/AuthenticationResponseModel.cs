using Org.BouncyCastle.Bcpg;

namespace DataAccess.Models.Authentication;

public class AuthenticationResponseModel : ApiResponse
{
    public string? Role { get; set; }
}