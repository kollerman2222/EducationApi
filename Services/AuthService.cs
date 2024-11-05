using Azure.Core;
using FgssrApi.CustomMethodsServices;
using FgssrApi.Data;
using FgssrApi.Dtos;
using FgssrApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
//using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static FgssrApi.Models.JwtConfig;

namespace FgssrApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IEmailSystem _emailSystem;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtConfig> jwtConfig , ApplicationDbContext context, ITokenService tokenService, IEmailSystem emailSystem, IWebHostEnvironment webHostEnvironment)
        {
            _jwtConfig = jwtConfig.Value;
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _emailSystem = emailSystem;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/users";

        }

        public async Task<string> RegisterAsync(RegisterDto registerdto)
        {
            if (await _userManager.FindByEmailAsync(registerdto.Email) != null)
            {
                return "Email is already registered!";

            }

            var user = new ApplicationUser
            {
                UserName = new MailAddress(registerdto.Email).User,
                Email = registerdto.Email,
                FullNameArabic = registerdto.FullNameArabic,
                FullNameEnglish = registerdto.FullNameEnglish,
                Address = registerdto.Address,
                Gender = registerdto.Gender,
                //BirthDate = registerdto.BirthDate,
                PhoneNumber = registerdto.PhoneNumber,
                PhoneConfirmed = false

            };


            var result = await _userManager.CreateAsync(user, registerdto.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;

                foreach (var err in result.Errors)
                {
                    errors = err.Description + Environment.NewLine;
                }

                return errors;

            }

            await _userManager.AddToRoleAsync(user, "User");

            return "User Created Succesfully";
            
        }


        public async Task<AuthDto> LoginAsync(LoginDto logindto)
        {
            var authDto = new AuthDto();

            var user = await _userManager.FindByEmailAsync(logindto.Email);
            var checkPassword = await _userManager.CheckPasswordAsync(user, logindto.Password);

            if (user == null || !checkPassword )
            {
                authDto.Message = "Email or Password is incorrect!";
                authDto.Token=string.Empty;
                return authDto;
            }

            //var token = CreateJwtAccessToken(user);
            //var refreshToken = CreateJwtRefreshToken(user, token).Result;

            var token = _tokenService.CreateJwtAccessToken(user);
            var refreshToken = _tokenService.CreateJwtRefreshToken(user, token).Result;


            authDto.Message = "Login is succesfull";
            authDto.Token = token;
            authDto.RefreshToken = refreshToken;
            return authDto;

        }


        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var getRefreshToken = await _context.RefreshToken.FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (getRefreshToken == null)
            {
                return false;
            }

            _context.RefreshToken.Remove(getRefreshToken);
            await _context.SaveChangesAsync();


            return true;
        }



        public async Task<ChangePasswordDto> ChangePasswordAsync(ChangePasswordDto cpd , string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ChangePasswordDto
                {
                    Message = $"User not found",
                    isChanged = false,
                };
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, cpd.OldPassword, cpd.NewPassword);
            if (!changePasswordResult.Succeeded)
            {

                return new ChangePasswordDto
                {
                    Message = "Error Wrong Password",
                    isChanged = false,
                };
            }

            return new ChangePasswordDto
            {
                Message = "Your password has been changed.",
                isChanged = true,
            };
            
        }


        public async Task<ChangePhoneNumberDto> ChangePhoneNumberAsync(ChangePhoneNumberDto cpnd, string? userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ChangePhoneNumberDto
                {
                    Message = $"User not found",
                    isChanged = false,
                };
            }

            user.PhoneVerifyCode = null;
            user.PhoneConfirmed = false;
            user.PhoneNumber = cpnd.NewPhoneNumber;

           var updateNumber = await _userManager.UpdateAsync(user);
           if(!updateNumber.Succeeded)
            {
                return new ChangePhoneNumberDto
                {
                    Message = "Something went wrong.",
                    isChanged = false,
                };
            }

         
            return new ChangePhoneNumberDto
            {
                Message = "Your PhoneNumber has been changed.",
                isChanged = true,
            };

        }



        public async Task<ChangeEmailDto> ChangeEmailAsync(ChangeEmailDto ced, string? userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ChangeEmailDto
                {
                    Message = $"User not found",
                    isChanged = false,
                };
            }

            user.EmailVerifyCode = null;
            user.EmailConfirmed = false;
            user.EmailCodeCreated = DateTime.MinValue;
            user.Email = ced.NewEmail;
            user.UserName = new MailAddress(ced.NewEmail).User;

            var updateEmail = await _userManager.UpdateAsync(user);
            if (!updateEmail.Succeeded)
            {
                return new ChangeEmailDto
                {
                    Message = "Something went wrong.",
                    isChanged = false,
                };
            }


            return new ChangeEmailDto
            {
                Message = "Your Email has been changed.",
                isChanged = true,
            };

        }



        public async Task<string> CreateVerifyEmailAsync(string? userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return $"User not found";
            }

            var code = VerifyCodeGenerator.GenerateRandomCode(111111, 999999);
            var emailSend = await _emailSystem.SendEmail(user.Email, "Email Verification Code", code, user.FullNameEnglish);
            if (!emailSend)
            {
                return "Something went wrong.";
            }
            user.EmailVerifyCode = code;
            user.EmailCodeCreated = DateTime.Now;

            var updateVerify = await _userManager.UpdateAsync(user);
            if (!updateVerify.Succeeded)
            {
                return "Something went wrong.";
            }

            return "Email Verification Code was sent successfully pls check your Email";

        }



        public async Task<string> ConfirmVerifyEmailAsync(string? userName , string verifyCode)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return $"User not found";
            }

            if (verifyCode != user.EmailVerifyCode)
            {
                return "Something went wrong.";
            }

            user.EmailConfirmed = true;
            user.EmailCodeCreated = DateTime.MinValue;
            user.EmailVerifyCode = null;

            var updateVerify = await _userManager.UpdateAsync(user);
            if (!updateVerify.Succeeded)
            {
                return "Something went wrong.";


            }

            return "Email Verification is successfull";

        }



        public async Task<UserProfileDto> GetUserDataAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new UserProfileDto
                {
                    Message = "User  not found"
                };
            }

            return new UserProfileDto
            {
                CurrentEmail = user.Email,
                Username = user.UserName,
                FullNameArabic = user.FullNameArabic,
                FullNameEnglish = user.FullNameEnglish,
                Gender = user.Gender,
                Address = user.Address,
                BirthDate = user.BirthDate,
                ProfileImage = user.ProfileImage,
                PhoneNumber = user.PhoneNumber,
                StudentNumber = user.StudentNumber
            };
        }

        public async Task<UserProfileDto> UpdateUserDataAsync(UserProfileDto upd ,string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new UserProfileDto
                {
                    Message = "User  not found"
                };
            }

            var oldImage = user.ProfileImage;


            user.FullNameArabic = upd.FullNameArabic ?? user.FullNameArabic;
            user.FullNameEnglish = upd.FullNameEnglish ?? user.FullNameEnglish;
            user.Gender = upd.Gender ?? user.Gender;
            user.Address = upd.Address ?? user.Address;
            user.BirthDate = upd.BirthDate;
            user.PhoneNumber = upd.PhoneNumber ?? user.PhoneNumber;
            if (upd.EditProfileImage != null)
            {
                user.ProfileImage = await SaveNewProfileImage(upd.EditProfileImage);
                if (oldImage != null)
                {
                    var oldImageDelete = Path.Combine(_uploadFolderPath, oldImage);
                    File.Delete(oldImageDelete);
                }

            }
            await _userManager.UpdateAsync(user);
            

            var UpdatedUser = await _userManager.FindByNameAsync(userName);


            return new UserProfileDto
            {
                CurrentEmail = UpdatedUser.Email,
                Username = UpdatedUser.UserName,
                FullNameArabic = UpdatedUser.FullNameArabic,
                FullNameEnglish = UpdatedUser.FullNameEnglish,
                Gender = UpdatedUser.Gender,
                Address = UpdatedUser.Address,
                BirthDate = UpdatedUser.BirthDate,
                ProfileImage = UpdatedUser.ProfileImage,
                PhoneNumber = UpdatedUser.PhoneNumber,
                StudentNumber = UpdatedUser.StudentNumber,
                Message="User Profile Updated Sucessfully"
            };
        }


        public async Task<string> SaveNewProfileImage(IFormFile image)
        {

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await image.CopyToAsync(stream);

            return imageName;
        }



        //private string CreateJwtAccessToken(ApplicationUser user)
        //{

        //    var tokenHandler = new JsonWebTokenHandler();
        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new List<Claim>
        //    {
        //        //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // was bugged doesnt add to jwt id with old package .tokens.jwt
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //        new Claim("Userid", user.Id),
        //        new Claim(JwtRegisteredClaimNames.Name, user.UserName),
        //        //new Claim("jti", Guid.NewGuid().ToString()),
        //    };

        //    var userRoles =  _userManager.GetRolesAsync(user).Result;

        //    foreach (var role in userRoles)
        //    {
        //        claims.Add(new Claim("role", role));
        //    }


        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddMinutes(1),
        //        SigningCredentials = signingCredentials,
        //        Issuer = _jwtConfig.Issuer,
        //        Audience = _jwtConfig.Audience,
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    //return tokenHandler.WriteToken(token);
        //    return token;

        //}



        //private async Task<string> CreateJwtRefreshToken(ApplicationUser user , string jwtAccessToken)
        //{
        //    var jwtTokenHandler = new JsonWebTokenHandler();
        //    var accessToken = jwtTokenHandler.ReadJsonWebToken(jwtAccessToken);
        //    var jwtId = accessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

        //    var refToken = new RefreshToken
        //    {
        //        Token = GenerateRefreshToken(),
        //        JwtAccessId = jwtId,
        //        UserId = user.Id,
        //        CreatedAt = DateTime.UtcNow,
        //        ExpiryDate = DateTime.UtcNow.AddMinutes(7),
        //    };

        //    // Save the refresh token to the database
        //   await _context.RefreshToken.AddAsync(refToken);
        //   await _context.SaveChangesAsync();

        //    return refToken.Token;
        //}

        //private string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using var rng = RandomNumberGenerator.Create();
        //    rng.GetBytes(randomNumber);
        //    return Convert.ToBase64String(randomNumber);

        //}





        //public async Task<ValidateTokenDto> ValidateTokenAndUpdateAsync(string accessToken , string refreshToken)
        //{
        //    var jwtTokenHandler = new JsonWebTokenHandler();

        //     var validationParameters = new TokenValidationParameters
        //    {
        //         ValidateIssuerSigningKey = true,
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = false,
        //         ValidIssuer = _jwtConfig.Issuer,
        //         ValidAudience = _jwtConfig.Audience,
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key)),
        //         ClockSkew = TimeSpan.Zero
        //     };

        //    try
        //    {
        //        var getClaimsFromAccessToken = await jwtTokenHandler.ValidateTokenAsync(accessToken, validationParameters);

        //        var jwtAccessToken = getClaimsFromAccessToken.SecurityToken as JsonWebToken;

        //        if (getClaimsFromAccessToken.IsValid)
        //        {
        //            var result = jwtAccessToken.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        //            if (!result)
        //            { 
        //                return new ValidateTokenDto
        //                {
        //                    Message = "Invalid Token",
        //                    Token = null,
        //                    isTokenValid = false,
        //                };
        //            } 
        //        }

        //        var getJwtId = jwtAccessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        //        var getUserId = jwtAccessToken.Claims.FirstOrDefault(c => c.Type == "Userid")?.Value;

        //        if (getJwtId == null || getUserId == null)
        //        {
        //            return new ValidateTokenDto
        //            {
        //                Message = "Wrong Token",
        //                Token = null,
        //                isTokenValid = false,
        //            };
        //        }

        //        var getRefreshToken = await _context.RefreshToken.FirstOrDefaultAsync(r => r.Token == refreshToken);

        //        if(getRefreshToken == null || getRefreshToken.UserId != getUserId || getRefreshToken.JwtAccessId != getJwtId)
        //        {
        //            return new ValidateTokenDto
        //            {
        //                Message = "Invalid Token",
        //                Token = null,
        //                isTokenValid = false,
        //            };
        //        }


        //        if (getRefreshToken.ExpiryDate < DateTime.UtcNow)
        //        {
        //            return new ValidateTokenDto
        //            {
        //                Message = "Refresh Token Expired",
        //                Token = null,
        //                isTokenValid = false,
        //            };
        //        }

        //        var getUser = await _userManager.FindByIdAsync(getUserId);

        //        var newAccessToken = CreateJwtAccessToken(getUser);

        //        var getJwtIdFromNewAcessToken = jwtTokenHandler.ReadJsonWebToken(newAccessToken);

        //        getRefreshToken.JwtAccessId = getJwtIdFromNewAcessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        //        await _context.SaveChangesAsync();

        //        return new ValidateTokenDto
        //        {
        //            Message = "AccessToken  sucessfully renewed",
        //            Token = newAccessToken,
        //            isTokenValid = true,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ValidateTokenDto
        //        {
        //            Message = ex.Message,
        //            Token = null,
        //            isTokenValid = false,
        //        };
        //    }

        //}



















    }
}
