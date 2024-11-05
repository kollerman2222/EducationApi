using FgssrApi.Data;
using FgssrApi.Dtos;
using FgssrApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FgssrApi.Services
{
    public class TokenService:ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _context;
        public TokenService(UserManager<ApplicationUser> userManager, IOptions<JwtConfig> jwtConfig, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
        }


        public string CreateJwtAccessToken(ApplicationUser user)
        {

            var tokenHandler = new JsonWebTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // was bugged doesnt add to jwt id with old package .tokens.jwt
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Userid", user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                //new Claim("jti", Guid.NewGuid().ToString()),
            };

            var userRoles = _userManager.GetRolesAsync(user).Result;

            foreach (var role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = signingCredentials,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
            return token;

        }



        public async Task<string> CreateJwtRefreshToken(ApplicationUser user, string jwtAccessToken)
        {
            var jwtTokenHandler = new JsonWebTokenHandler();
            var accessToken = jwtTokenHandler.ReadJsonWebToken(jwtAccessToken);
            var jwtId = accessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var refToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                JwtAccessId = jwtId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(7),
            };

            // Save the refresh token to the database
            await _context.RefreshToken.AddAsync(refToken);
            await _context.SaveChangesAsync();

            return refToken.Token;
        }





        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }




        public async Task<ValidateTokenDto> ValidateTokenAndUpdateAsync(string accessToken, string refreshToken)
        {
            var jwtTokenHandler = new JsonWebTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key)),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var getClaimsFromAccessToken = await jwtTokenHandler.ValidateTokenAsync(accessToken, validationParameters);

                var jwtAccessToken = getClaimsFromAccessToken.SecurityToken as JsonWebToken;

                if (getClaimsFromAccessToken.IsValid)
                {
                    var result = jwtAccessToken.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return new ValidateTokenDto
                        {
                            Message = "Invalid Token",
                            Token = null,
                            isTokenValid = false,
                        };
                    }
                }

                var getJwtId = jwtAccessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var getUserId = jwtAccessToken.Claims.FirstOrDefault(c => c.Type == "Userid")?.Value;

                if (getJwtId == null || getUserId == null)
                {
                    return new ValidateTokenDto
                    {
                        Message = "Wrong Token",
                        Token = null,
                        isTokenValid = false,
                    };
                }

                var getRefreshToken = await _context.RefreshToken.FirstOrDefaultAsync(r => r.Token == refreshToken);

                if (getRefreshToken == null || getRefreshToken.UserId != getUserId || getRefreshToken.JwtAccessId != getJwtId)
                {
                    return new ValidateTokenDto
                    {
                        Message = "Invalid Token",
                        Token = null,
                        isTokenValid = false,
                    };
                }


                if (getRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new ValidateTokenDto
                    {
                        Message = "Refresh Token Expired",
                        Token = null,
                        isTokenValid = false,
                    };
                }

                var getUser = await _userManager.FindByIdAsync(getUserId);

                var newAccessToken = CreateJwtAccessToken(getUser);

                var getJwtIdFromNewAcessToken = jwtTokenHandler.ReadJsonWebToken(newAccessToken);

                getRefreshToken.JwtAccessId = getJwtIdFromNewAcessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                await _context.SaveChangesAsync();

                return new ValidateTokenDto
                {
                    Message = "AccessToken  sucessfully renewed",
                    Token = newAccessToken,
                    isTokenValid = true,
                };
            }
            catch (Exception ex)
            {
                return new ValidateTokenDto
                {
                    Message = ex.Message,
                    Token = null,
                    isTokenValid = false,
                };
            }

        }




    }
}
