using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auction.BLL.Configure
{
    /// <summary>
    /// Authentification options for TWT Token 
    /// </summary>
    public class AuthOptions
    {
        public const string ISSUER = "https://localhost:44325";
        public const string AUDIENCE = "https://localhost:44325"; 
        private const string KEY = "superSecretKey@345";
        public const int LIFETIME = 60 * 6; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
