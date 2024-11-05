using FgssrApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;

namespace FgssrApi.CustomMethodsServices
{
    public class CheckJwtExpire
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;
        private readonly JwtConfig _jwtConfig;

        public CheckJwtExpire(RequestDelegate next, HttpClient httpClient, IOptions<JwtConfig> jwtConfig)
        {
            _next = next;
            _httpClient = httpClient;
            _jwtConfig = jwtConfig.Value;
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.Cookies["AuthAccessToken"]))
            {
                var encryptedJwtTokenCookie = context.Request.Cookies["AuthAccessToken"];

                var decryptedJwtTokenCookie = SymmetricEncryption.Decrypt(encryptedJwtTokenCookie,_jwtConfig.JwtCookieEncryptionKey);

                var jwtTokenHandler = new JsonWebTokenHandler();
                var jwtToken =  jwtTokenHandler.ReadJsonWebToken(decryptedJwtTokenCookie);
                var expirationTime = jwtToken.ValidTo;

                
                var timeRemaining = DateTime.UtcNow - expirationTime;

                if (timeRemaining.TotalSeconds < 20)
                {
                    var refreshResponse = await _httpClient.PostAsync("http://localhost:61449/api/UserAuth/refreshtoken", new StringContent(string.Empty));
                }
                
            }

            await _next(context);
        }
    }
}
