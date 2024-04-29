using Microsoft.AspNetCore.Identity;

namespace ReceipeGenerator.Model;

    public class ApplicationUser:IdentityUser
{
    public string RefreshToken { get; set; }
    public System.DateTime? RefreshTokenExpiryTime { get; set; }
}

