using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace WebApplication2
{
    public class AuthOptions
    {
        public const string ISSUER = "ExampleBack"; // издатель токена
        public const string AUDIENCE = "ExampleClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации, лучше сгенерить большой
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}