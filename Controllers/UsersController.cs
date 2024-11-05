using FgssrApi.CustomMethodsServices;
using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.UnitOFWork;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FgssrApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string? _uploadFolderPath;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {

            var users = await _userManager.Users.ToListAsync();

            var getUsers = new List<UsersInfoDto>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);


                var userModel = new UsersInfoDto
                {
                    UserId = user.Id,
                    FullNameEnglish = user.FullNameEnglish,
                    FullNameArabic = user.FullNameArabic,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ApplicationStatus = user.ApplicationStatus,
                    ProfileImage = user.ProfileImage,
                    SectionId = user.SectionId,
                    SectionName = await _unitofwork.Sections.GetNamebyIdAsync(user.SectionId),
                    Roles = userRoles.Select(roleName => new RoleDto
                    {
                        RoleName = roleName
                    }).ToList()
                };

                getUsers.Add(userModel);
            }

            return Ok(getUsers);

        }


        [HttpGet]
        [Route("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return NotFound();
            }

            var getUser = new UsersInfoDto
            {

                UserId = user.Id,
                FullNameEnglish = user.FullNameEnglish,
                FullNameArabic = user.FullNameArabic,
                BirthDate = user.BirthDate,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ApplicationStatus = user.ApplicationStatus,
                ProfileImage = user.ProfileImage,
                SectionId = user.SectionId,
                SectionName = await _unitofwork.Sections.GetNamebyIdAsync(user.SectionId),
                Roles = userRoles.Select(roleName => new RoleDto
                {
                    RoleName = roleName
                }).ToList()

            };

            return Ok(getUser);
        }


        [HttpPut]
        [Route("UpdateUserRoleById/{userId}")]
        public async Task<IActionResult> UpdateUserRoleById(string userId , [FromBody] string[] roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roleName)
            {
                //if (roles.Any(r => r == role))
                //    await _userManager.RemoveFromRoleAsync(user, role);

                if (!roles.Any(r => r == role))
                    await _userManager.AddToRoleAsync(user, role);
            }

            //user.PhoneNumber = UVM.PhoneNumber;
            //user.FullNameEnglish = UVM.FullNameEnglish;
            //user.ApplicationStatus = UVM.ApplicationStatus;

            await _userManager.UpdateAsync(user);

            return Ok("User role is updated sucessfully");

        }




        [HttpPost]
        [Route("UserProgramAdmission")]
        public async Task<IActionResult> UserProgramAdmission([FromForm] AdmissionDto AVM)
        {
            var user = await _userManager.FindByIdAsync(AVM.userId);
            if (user == null)
            {
                return NotFound();
            }
        
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/gradcerts/{user.FullNameEnglish}";
            Directory.CreateDirectory(_uploadFolderPath);
            user.ApplicationStatus = "Pending";
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(AVM.GraduationCert.FileName)}";
            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);
            using var stream = System.IO.File.Create(imageSaveLocation);
            await AVM.GraduationCert.CopyToAsync(stream);
            user.GraduationCertUpload = imageName;
            user.SectionId = AVM.sectionId;
            var AdmissionNumber = AdmissionNumberGenerator.GenerateAdmissionNumber(11111, 99999);
            user.AdmissionNumber = AdmissionNumber;
            await _userManager.UpdateAsync(user);
            return Ok("User admission was sucessfull");

        }





        [HttpDelete]
        [Route("DeleteUserById/{userId}")]
        public async Task<IActionResult> DeleteUserById(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return Ok($"User {user.FullNameEnglish} was deleted sucessfully");
        }



    }
}
