using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
namespace WebApiDemo.Utility
{

    public class JwtTokenManager
        {
        private const string SecretKey = "this is my custom Secret key for authentication";
        private const int ExpirationMinutes = 60; // Token expiration time in minutes

        // Generate JWT Token
        public static string GenerateToken(string Email)
            {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "localhost:44359",
                audience: "localhost:44359",
                claims: claims,
                expires: DateTime.Now.AddMinutes(ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            }

        // Validate JWT Token
        public static ClaimsPrincipal ValidateToken(string token)
            {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
                {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = "localhost:44359",
                ValidAudience = "localhost:44359",
                IssuerSigningKey = securityKey
                };

            try
                {
                return tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                }
            catch (Exception)
                {
                return null;
                }
            }
        }
    }