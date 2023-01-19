using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Authentication.Utils;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Authentication.JWT
{
    public class JWTHelper : ITokenHelper
    {
        /// <summary>
        /// IConfiguration Wep API de  appSettings.json da yapılır.
        /// </summary>
        public IConfiguration _configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        public JWTHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            // Konfigrasyon (.config) dosyamdaki token konfigrasyon ayarlarını okudum.
            _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        public EntityResult<AccessToken> CreateToken(User user, List<Permission> permissions)
        {

            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(
                _tokenOptions,
                user,
                signingCredentials,
                permissions
                );
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new EntityResult<AccessToken>
            {
                Entity = new AccessToken
                {
                    Token = "Bearer "+ jwtSecurityTokenHandler,
                    Expiration = _accessTokenExpiration,

                }
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, List<Permission> permissions)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: DateTime.Now.AddYears(1),
                notBefore: DateTime.Now,
                claims: SetClaims(user, permissions),
                signingCredentials: signingCredentials
                );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<Permission> permissions)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.ID.ToString());
            claims.AddEmail(user.Email);
            claims.AddName(user.UserName);
            claims.AddRoles(permissions.Select(o => o.PermissionName).ToArray());

            return claims;
        }
    }
}
