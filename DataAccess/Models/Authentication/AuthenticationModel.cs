namespace DataAccess.Models.Authentication;

public class AuthenticationModel
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }

    public string[] Roles { get; set; }
    
}