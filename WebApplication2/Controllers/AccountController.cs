using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication2.Models; // класс Person

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных
        private List<Person> people = new List<Person>
        {
            new Person {Id = 1, Login="admin@gmail.com", Password="12345", Claims = new string[] {"users", "admin" } },
            new Person {Id = 2, Login="qwerty@gmail.com", Password="55555", Claims = new string[] { "sync" } }
        };

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
           
            if (person == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            var identity = GetIdentity(person);
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                claims = person.Claims, 
                id = person.Id,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(Person person)
        {
           
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                 
                };
            claims.AddRange(person.Claims.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            
                return claimsIdentity;
           
        }
    }
}