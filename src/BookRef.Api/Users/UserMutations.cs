using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookRef.Api.Common;
using BookRef.Api.Extensions;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using FluentValidation;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.IdentityModel.Tokens;

namespace BookRef.Api.Users
{
    public record NewUserInput(string Username, string Password, string Email);
    public record SingInInput(string Username, string Password);

    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations
    {
        [UseApplicationDbContext]
        public async Task<Payload<Guid>> NewUserAsync(
             NewUserInput input,
             [ScopedService] BookRefDbContext context)
        {
            var user = new User(input.Username, input.Email);
            user.SetPassword(input.Password);
            await context.Users.AddAsync(user);
            var library = new PersonalLibrary(Guid.NewGuid(), user);
            await context.Libraries.AddAsync(library);
            await context.SaveChangesAsync();

            return new Payload<Guid>(library.Id);
        }

        [UseApplicationDbContext]
        public async Task<Payload<string>> SingInAsync(
             SingInInput input,
             [ScopedService] BookRefDbContext context)
        {
            var user = context.Users.First(e => e.Username == input.Username);
            var isValid = BCrypt.Net.BCrypt.Verify(input.Password, user.Password);

            return new Payload<string>(BuildToken(user.Username, user.PersonalLibraryId));
        }

        private string BuildToken(string username, Guid librarId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvFactory.GetJwtKey()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            var claimUser = new Claim(ClaimTypes.Name, username);
            claims.Add(claimUser);
            var claimLibrary = new Claim("LibraryId", librarId.ToString());
            claims.Add(claimLibrary);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(500),
                SigningCredentials = creds,
                Issuer = EnvFactory.GetJwtIssuer(),
                Audience = EnvFactory.GetJwtIssuer()
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

    }
}
