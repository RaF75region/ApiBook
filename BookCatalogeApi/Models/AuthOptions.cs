using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookCatalogeApi.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "localhost";
        public const string AUDIENCE = "localhost";
        const string KEY = "secretKey_super_key!111";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}

