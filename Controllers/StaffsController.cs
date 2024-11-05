using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FgssrApi.Data;
using FgssrApi.Models;
using FgssrApi.UnitOFWork;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FgssrApi.Dtos;

namespace FgssrApi.Controllers
{
    //[Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/[controller]")]
    public class StaffsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public StaffsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        [HttpGet]
        [Route("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {

            var staffs = await _unitofwork.Staffs.GetAllAsync();
            var svm = staffs.Select(staff => new StaffsDto
            {
                StID=staff.StaffId,
                Biograpghy = staff.Biograpghy,
                StaffName = staff.StaffName,
                StaffPosition = staff.StaffPosition,
                StaffImageName = staff.StaffImage,
                Email=staff.Email,
            }).ToList();

            return Ok(svm);
        }


        [HttpGet]
        [Route("GetStaffById/{id}")]
        public async Task<IActionResult> GetStaffById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            var svm = new StaffsDto()
            {
                StID =staff.StaffId,
                Biograpghy = staff.Biograpghy,
                StaffName = staff.StaffName,
                StaffPosition = staff.StaffPosition,
                StaffImageName = staff.StaffImage,
                Email = staff.Email,
            };
            return Ok(svm);
        }

        [HttpDelete]
        [Route("DeleteStaffById/{id}")]
        public async Task<IActionResult> DeleteStaff(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            _unitofwork.Staffs.Delete(staff);
            var imageDelete = Path.Combine(_unitofwork.Staffs.uploadFolderPublic, staff.StaffImage);
            System.IO.File.Delete(imageDelete);
            _unitofwork.SaveChanges();
            return Ok("Delete is successfull");
        }


      

        [HttpPost]
        [Route("CreateNewStaff")]
        public async Task<IActionResult> CreateStaff([FromForm] StaffsDtoCU SVM)
        {

           var staff= await _unitofwork.Staffs.CreateAsync(SVM);
            _unitofwork.SaveChanges();

            return CreatedAtAction(nameof(GetStaffById), new { id = staff.StaffId}, SVM);
        }

    

        [HttpPut]
        [Route("UpdateStaffById/{id}")]
        public async Task<IActionResult> UpdateStaff(int? id, [FromForm] StaffsDtoCU SVM)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var oldImage = staff.StaffImage;

            staff.StaffName=SVM.StaffName;
            staff.StaffPosition=SVM.StaffPosition;
            staff.Email=SVM.Email;
            staff.Biograpghy=SVM.Biograpghy;
         
            if (SVM.UploadImage != null)
            {
                staff.StaffImage = await _unitofwork.Staffs.SaveNewImage(SVM.UploadImage);
                var oldImageDelete = Path.Combine(_unitofwork.Staffs.uploadFolderPublic, oldImage);
                System.IO.File.Delete(oldImageDelete);
            }

            _unitofwork.SaveChanges();

            return Ok(staff);
        }
    }
}
