namespace DataAccess.Models.Token;

public class RefreshTokenDto
{
    public string Id { get; set; }

    public string JwtId { get; set; }
    
    public string UserId { get; set; }
    
    public string Token { get; set; }
    
    public Boolean? IsUsed { get; set; }
    
    public Boolean? IsRevoked { get; set; }
    
    public string UserRole { get; set; }

    public DateTime? IssuedAt { get; set; }
    
    public DateTime? ExpiredAt { get; set; }
}