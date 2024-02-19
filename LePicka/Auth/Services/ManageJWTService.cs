using Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Services
{
    public interface IManageJWTService
    {
        public AuthenticationResponse BuildToken(UserCredentials userCredentials,
            IEnumerable<Claim> claims,
            SymmetricSecurityKey key,
            DateTime expiration);
    }

    public class ManageJWTService : IManageJWTService
    {
        public AuthenticationResponse BuildToken(UserCredentials userCredentials,
            IEnumerable<Claim> claims,
            SymmetricSecurityKey key,
            DateTime expiration)
        {

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
