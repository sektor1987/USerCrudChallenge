using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserCrudApiChallenge.Application.Interface;

namespace Auth.Demo
{
    public interface IJWTAuthenticationManager
    {
        System.Threading.Tasks.Task<AuthenticationResponse> AuthenticateAsync(string username, string password);
        IDictionary<string, string> UsersRefreshTokens { get; set; }
        AuthenticationResponse Authenticate(string username, Claim[] claims);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
      

        public IDictionary<string, string> UsersRefreshTokens { get; set; }
        private readonly IUserAplication _userAplication;
        private readonly string tokenKey;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        public JWTAuthenticationManager(string tokenKey, IRefreshTokenGenerator refreshTokenGenerator, IUserAplication userAplication)
        {
            this.tokenKey = tokenKey;
            this.refreshTokenGenerator = refreshTokenGenerator;
            UsersRefreshTokens = new Dictionary<string, string>();
            _userAplication = userAplication;

        }

        public AuthenticationResponse Authenticate(string username, Claim[] claims)
        {
            var token = GenerateTokenString(username, DateTime.UtcNow, claims);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            if (UsersRefreshTokens.ContainsKey(username))
            {
                UsersRefreshTokens[username] = refreshToken;
            }
            else
            {
                UsersRefreshTokens.Add(username, refreshToken);
            }

            return new AuthenticationResponse
            {
                JwtToken = token,
                RefreshToken = refreshToken
            };
        }

        public async System.Threading.Tasks.Task<AuthenticationResponse> AuthenticateAsync(string username, string password)
        {
             bool exist = await _userAplication.ValidateUserLogin(username, password);

            if (!exist)
            {
                return null;
            }

            var token = GenerateTokenString(username, DateTime.UtcNow);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            if (UsersRefreshTokens.ContainsKey(username))
            {
                UsersRefreshTokens[username] = refreshToken;
            }
            else
            {
                UsersRefreshTokens.Add(username, refreshToken);
            }

            return new AuthenticationResponse
            {
                JwtToken = token,
                RefreshToken = refreshToken
            };
        }

        string GenerateTokenString(string username, DateTime expires, Claim[] claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                 claims ?? new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                //NotBefore = expires,
                Expires = expires.AddMinutes(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}