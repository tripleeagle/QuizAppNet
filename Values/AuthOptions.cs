using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace QuizappNet.Values
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // token source
        public const string AUDIENCE = "http://localhost:51884/"; //consumer of the token
        const string KEY = "mysupersecret_secretkey!123";   
        public const int LIFETIME = 1; // 1 minute
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}