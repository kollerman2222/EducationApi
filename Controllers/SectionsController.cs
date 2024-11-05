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
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Authorization;
using FgssrApi.Dtos;

namespace FgssrApi.Controllers
{
    //[Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public SectionsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        [HttpGet]
        [Route("GetAllProgramsByDepartmentName/{departmentName}")]
        public async Task<IActionResult> GetAllProgramsByDepartmentName(string departmentName)
        {

            var sections = await _unitofwork.Sections.GetAllAsync();
            var filterSections = sections.Where(s => s.Department?.DepartmentName == departmentName);
            var dsvm = filterSections.Select(sec => new DiplomasSectionsDto
            {
              SectionName = sec.SectionName,
              SecId=sec.SectionId,
              Description = sec.Description,
              isActive=sec.isActive,
              DepartmentId=sec.DepartmentId,
              SectionImage=sec.SectionImage,
            }).ToList();

            return Ok(dsvm);
        }

        [HttpGet]
        [Route("GetProgramById/{id}")]
        public async Task<IActionResult> GetProgramById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var section = await _unitofwork.Sections.GetByIdAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            var dsvm = new DiplomasSectionsDto()
            {
                SectionName = section.SectionName,
                SecId = section.SectionId,
                Description = section.Description,
                isActive = section.isActive,
                DepartmentId =section.DepartmentId,
                SectionImage = section.SectionImage
            };
            return Ok(dsvm);
        }

        [HttpDelete]
        [Route("DeleteProgramById/{id}")]
        public async Task<IActionResult> DeleteProgram(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var section = await _unitofwork.Sections.GetByIdAsync(id);
            var getDepartmentName = section?.Department?.DepartmentName;
            if (section == null)
            {
                return NotFound();
            }
            _unitofwork.Sections.Delete(section);
            var imageDelete = Path.Combine(_unitofwork.Sections.uploadFolderPublic, section.SectionImage);
            System.IO.File.Delete(imageDelete);
            _unitofwork.SaveChanges();

            return Ok("Delete is successfull");
        }


      

        [HttpPost]
        [Route("CreateNewProgram")]
        public async Task<IActionResult> CreateNewProgram([FromForm] DiplomasSectionsDtoCU DSVM)
        {


            var sec = await _unitofwork.Sections.CreateAsync(DSVM);
            _unitofwork.SaveChanges();
           
            return CreatedAtAction(nameof(GetProgramById), new { id = sec.SectionId }, DSVM);
        }

       

        [HttpPut]
        [Route("UpdateProgramById/{id}")]
        public async Task<IActionResult> UpdateProgramById(int? id, [FromForm]DiplomasSectionsDtoCU DSVM)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var sections = await _unitofwork.Sections.GetByIdAsync(id);
            if (sections == null)
            {
                return NotFound();
            }

            var oldImage = sections.SectionImage;

            sections.SectionName=DSVM.SectionName;
            sections.Description=DSVM.Description;
            sections.isActive=DSVM.isActive;
            sections.DepartmentId=DSVM.DepartmentId;

            if (DSVM.UploadImage != null)
            {
                sections.SectionImage = await _unitofwork.Sections.SaveNewImage(DSVM.UploadImage);
                var oldImageDelete = Path.Combine(_unitofwork.Sections.uploadFolderPublic, oldImage);
                System.IO.File.Delete(oldImageDelete);
            }

            _unitofwork.SaveChanges();

            return Ok(sections);
        }



        //[HttpGet]
        //[Route("GetAllProgramsAndPaginateByDepartment")]
        //public async Task<IActionResult> PaginateProgramsByDepartment([FromQuery] string departmentSearch, int pageNumber = 1)
        //{
        //    var sections = await _unitofwork.Sections.GetAllAsync();
        //    if (!String.IsNullOrEmpty(departmentSearch))
        //    {
        //        sections = sections.Where(x => x.Department?.DepartmentName == departmentSearch);
        //    }
        //    else
        //    {
        //        return Ok("departmentSearch parameter is empty");
        //    }
           
        //    var totalItems = sections.Count();
        //    var totalPages = (int)Math.Ceiling((decimal)totalItems / 6);
        //    var pagination = sections.Skip((pageNumber - 1) * 6).Take(6).ToList();
           

        //    var dsvm = new DiplomasSectionsViewModel
        //    {
        //        secs = pagination,
        //        TotalPages = totalPages,
        //        Page = pageNumber,
        //        departmentSearch = departmentSearch,
                
        //    };
        //    return Ok(dsvm);
        //}

        //[HttpGet]
        //[Route("GetAllProgramsAndPaginateBySearch")]
        //public async Task<IActionResult> PaginateProgramsBySearch([FromQuery] string query,[FromQuery] int pageNumber = 1)
        //{
        //    var sections = await _unitofwork.Sections.GetAllAsync();
        //    if (!String.IsNullOrEmpty(query))
        //    {
        //        sections = sections.Where(x => x.SectionName.Contains(query));
        //    }
        //    else
        //    {
        //        return Ok("query search parameter is empty");
        //    }

        //    var totalItems = sections.Count();
        //    var totalPages = (int)Math.Ceiling((decimal)totalItems / 6);
        //    var pagination = sections.Skip((pageNumber - 1) * 6).Take(6).ToList();


        //    var dsvm = new DiplomasSectionsViewModel
        //    {
        //        secs = pagination,
        //        TotalPages = totalPages,
        //        Page = pageNumber,
        //        query = query

        //    };
        //    return Ok(dsvm);
        //}




    }
}
