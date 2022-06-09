namespace tutoring_online_be.Controllers.Utils;

public class AppSetting
{
    public string SecretKey { get; set; }
    
    public int AccessTokenExpired { get; set; }
    
    public int RefreshTokenExpired { get; set; }
}