using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.UnitOFWork;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FgssrApi.Controllers
{

    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]

    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;

        public DepartmentsController(IUnitOfWork unitofwork) 
        {
            _unitofwork = unitofwork;
        }

        [HttpGet]
        [Route("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {

            var departments = await _unitofwork.DiplomasDepartments.GetAllAsync();
            var ddvm = departments.Select(dep => new DiplomasDepartmentsDto
            {
                DepId = dep.DepartmentId,
                DepartmentName = dep.DepartmentName,
                Description = dep.Description,
                Sections = dep.Sections.Select(s=> new DiplomasSectionsDto { 

                    SecId=s.SectionId,
                    SectionImage=s.SectionImage,
                    SectionName=s.SectionName,
                    Description=s.Description,
                    DepartmentId=s.DepartmentId,
                    isActive=s.isActive
                }).ToList()
            }).ToList();
                  
            return Ok(ddvm);
        }

        [HttpGet]
        [Route("GetDepartmentById/{id}")]

        public async Task<IActionResult> GetDepartmentById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            var ddvm = new DiplomasDepartmentsDto()
            {
                DepId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description,
                Sections=department.Sections.Select(s=> new DiplomasSectionsDto { 
                    SecId=s.SectionId,
                    SectionImage=s.SectionImage,
                    SectionName=s.SectionName,
                    Description=s.Description,
                    DepartmentId=s.DepartmentId,
                    isActive =s.isActive
                }).ToList()
            };

            return Ok(ddvm);
        }

        [HttpDelete]
        [Route("DeleteDepartmentById/{id}")]

        public async Task<IActionResult> DeleteDepartment(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            _unitofwork.DiplomasDepartments.Delete(department);
            _unitofwork.SaveChanges();

            return Ok("Delete is successfull");
        }


        [HttpPost]
        [Route("CreateNewDepartment")]

        public async Task<IActionResult> CreateDepartment([FromBody] DiplomasDepartmentsDtoCU DDVM)
        {

           var dep = await _unitofwork.DiplomasDepartments.CreateAsync(DDVM);
            _unitofwork.SaveChanges();

            return CreatedAtAction(nameof(GetDepartmentById), new { id = dep.DepartmentId }, DDVM);
        }



        [HttpPut]
        [Route("UpdateDepartmentById/{id}")]
        public async Task<IActionResult> UpdateDepartment(int? id , [FromBody] DiplomasDepartmentsDtoCU ddvm)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var department = await _unitofwork.DiplomasDepartments.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.DepartmentName = ddvm.DepartmentName;
            department.Description = ddvm.Description;
            _unitofwork.SaveChanges();


            return Ok(department);
        }


    }
}
