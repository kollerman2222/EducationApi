using FgssrApi.CustomMethodsServices;
using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Net.Mail;
using System.Security.Claims;

namespace FgssrApi.Controllers
{   
    

    [ApiController]
    [Route("api/[controller]")]
    
    public class UserAuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly JwtConfig _jwtConfig;

        public UserAuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager , IAuthService authService , ITokenService tokenService, IOptions<JwtConfig> jwtConfig)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
            _tokenService = tokenService;
            _jwtConfig = jwtConfig.Value;
        }

 
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerdto)
        {
            //registerdto model validation done automatically in apicontroller

            var reg = await _authService.RegisterAsync(registerdto);

            return Ok(reg);

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto logindto)
        {
            //logindto model validation done automatically in apicontroller

            var log = await _authService.LoginAsync(logindto);
            var accessToken = log.Token;
            var encryptedAccessToken = SymmetricEncryption.Encrypt(accessToken, _jwtConfig.JwtCookieEncryptionKey);

            Response.Cookies.Append("AuthAccessToken", encryptedAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to false if not using HTTPS during development
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(1) // Set cookie expiration
            });

            Response.Cookies.Append("AuthRefreshToken", log.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to false if not using HTTPS during development
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(7) // Set cookie expiration
            });

            return Ok(log);

        }



        [Authorize]
        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> refreshToken()
        {

            var getAccessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = HttpContext.Request.Cookies["AuthRefreshToken"];

            var refresh = await _tokenService.ValidateTokenAndUpdateAsync(getAccessToken, refreshToken);

            if (refresh.isTokenValid == false)
            {
                return Unauthorized("Something Went Wrong");
            }

            Response.Cookies.Delete("AuthAccessToken");


            var newAccessToken = refresh.Token;
            var encryptedAccessToken = SymmetricEncryption.Encrypt(newAccessToken, _jwtConfig.JwtCookieEncryptionKey);


            Response.Cookies.Append("AuthAccessToken", encryptedAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to false if not using HTTPS during development
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(1) // Set cookie expiration
            });


            return Ok(refresh.Message);
        }


        [Authorize]
        [HttpDelete]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {

            var refreshToken = HttpContext.Request.Cookies["AuthRefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("RefreshToken not found");
            }

            var logout = await _authService.LogoutAsync(refreshToken);

            if(logout == false)
            {
                return Unauthorized("Something Went Wrong");
            }

            Response.Cookies.Delete("AuthAccessToken");
            Response.Cookies.Delete("AuthRefreshToken");

           

            return Ok("Logged out successfully");
        }

        [Authorize]
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto cpd)
        {

            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var password = await _authService.ChangePasswordAsync(cpd, user);

            if(password.isChanged == false)
            {
                return Ok("Error Wrong Password , please try again later");

            }

            return Ok("Your password has been changed sucessfully");

        }




        [Authorize]
        [HttpPut]
        [Route("ChangePhoneNumber")]
        public async Task<IActionResult> ChangePhoneNumber([FromForm] ChangePhoneNumberDto cpnd)
        {

            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var number = await _authService.ChangePhoneNumberAsync(cpnd, user);

            if (number.isChanged == false)
            {
                return Ok("Something went wrong , please try again later");

            }

            return Ok("Your PhoneNumber has been changed sucessfully");

        }



        [Authorize]
        [HttpPut]
        [Route("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromForm] ChangeEmailDto ced)
        {

            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var email = await _authService.ChangeEmailAsync(ced, user);

            if (email.isChanged == false)
            {
                return Ok("Something went wrong , please try again later");

            }

            return Ok("Your Email has been changed sucessfully");

        }


        [Authorize]
        [HttpPost]
        [Route("SendEmailVerificationCode")]
        public async Task<IActionResult> CreateVerifyEmail()
        {
            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var sendEmail = await _authService.CreateVerifyEmailAsync(user);
            
            return Ok(sendEmail);

        }

        [Authorize]
        [HttpPost]
        [Route("VerifyEmailwithCode")]
        public async Task<IActionResult> ConfirmVerifyEmail([FromForm] string verifyCode)
        {
            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var sendEmail = await _authService.ConfirmVerifyEmailAsync(user , verifyCode);

            return Ok(sendEmail);

        }


        [Authorize]
        [HttpGet]
        [Route("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if(user == null)
            {
                return BadRequest("User not found");
            }

            var getData = await _authService.GetUserDataAsync(user);

            return Ok(getData);

        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUserData")]
        public async Task<IActionResult> UpdateUserData([FromForm] UserProfileDto upd)
        {
            var user = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var getData = await _authService.UpdateUserDataAsync(upd,user);

            return Ok(getData);

        }







        [Authorize]
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> test()
        {

            return Ok("hello from authorize method");

        }
       
        [HttpGet]
        [Route("test2")]
        public async Task<IActionResult> test2()
        {

            string c = string.Empty;

            var us = User.Claims.ToList();
            foreach(var claim in us)
            {
                c += claim;
            }
            


            var test = "test authorization" + c;


            return Ok(test);

        }

    }
}
