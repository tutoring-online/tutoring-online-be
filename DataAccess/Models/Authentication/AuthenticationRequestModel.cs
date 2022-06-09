namespace DataAccess.Models.Authentication;

public class AuthenticationRequestModel
{
    public string Token { get; set; }
    
    public string? role { get; set; }
}