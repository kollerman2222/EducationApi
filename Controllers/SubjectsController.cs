using FgssrApi.Dtos;
using FgssrApi.UnitOFWork;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FgssrApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public SubjectsController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        [HttpGet]
        [Route("GetAllSubjectsByDepartmentName/{departmentName}")]
        public async Task<IActionResult> GetAllSubjectsByDepartmentName(string departmentName)
        {
           var subjects = await _unitofwork.Subjects.GetAllAsync();
           var filterSubjects = subjects.Where(s => s.Section?.Department?.DepartmentName == departmentName);
            var svm = filterSubjects.Select(ss => new SubjectsDto
            {
                SubjectNameArabic=ss.SubjectNameArabic,
                SubjectNameEnglish=ss.SubjectNameEnglish,
                ScientificDegree=ss.ScientificDegree,
                SubjectCode=ss.SubjectCode,
                SubjectId=ss.SubjectId,
                SubjectHours=ss.SubjectHours,
                SubjectSemester=ss.SubjectSemester,
                SectionName=ss.Section?.SectionName,
                DepartmentName=ss.Section?.Department?.DepartmentName
            }).ToList();

            return Ok(svm);
        }


        [HttpGet]
        [Route("GetAllSubjectsByProgramName/{programName}")]
        public async Task<IActionResult> GetAllSubjectsByProgramName(string programName)
        {
            if (programName == null)
            {
                return BadRequest();
            }

            var section = await _unitofwork.Sections.GetByNameAsync(programName);

            if (section == null)
            {
                return NotFound();
            }

            var subjects = await _unitofwork.Subjects.GetAllAsync();
            var filterSubjects = subjects.Where(s => s.SectionId == section?.SectionId);


            var svm = filterSubjects.Select(ss => new SubjectsDto
            {
                SubjectNameArabic = ss.SubjectNameArabic,
                SubjectNameEnglish = ss.SubjectNameEnglish,
                ScientificDegree = ss.ScientificDegree,
                SubjectCode = ss.SubjectCode,
                SubjectId = ss.SubjectId,
                SubjectHours = ss.SubjectHours,
                SubjectSemester = ss.SubjectSemester,
                SectionName = ss.Section?.SectionName,
                DepartmentName = ss.Section?.Department?.DepartmentName

            }).ToList();
            return Ok(svm);
        }


        [HttpGet]
        [Route("GetSubjectById/{id}")]
        public async Task<IActionResult> GetSubjectById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var subjects = await _unitofwork.Subjects.GetByIdAsync(id);

            if (subjects == null)
            {
                return NotFound();
            }

            var svm = new SubjectsDto
            {
                SubjectNameArabic = subjects.SubjectNameArabic,
                SubjectNameEnglish = subjects.SubjectNameEnglish,
                ScientificDegree = subjects.ScientificDegree,
                SubjectCode = subjects.SubjectCode,
                SubjectId = subjects.SubjectId,
                SubjectHours = subjects.SubjectHours,
                SubjectSemester = subjects.SubjectSemester,
                SectionName = subjects.Section?.SectionName,
                DepartmentName = subjects.Section?.Department?.DepartmentName

            };

            return Ok(svm);
        }




        [HttpPost]
        [Route("CreateNewSubject")]

        public async Task<IActionResult> CreateNewSubject([FromBody] SubjectsDtoCU SVM)
        {

            
            var sub = await _unitofwork.Subjects.CreateAsync(SVM);

            _unitofwork.SaveChanges();

      
            return CreatedAtAction(nameof(GetSubjectById), new { id = sub.SubjectId  }, SVM);
        }


        [HttpDelete]
        [Route("DeleteSubjectById/{id}")]
        public async Task<IActionResult> DeleteSubject(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var subject = await _unitofwork.Subjects.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            _unitofwork.Subjects.Delete(subject);
            _unitofwork.SaveChanges();

            return Ok("Delete is successfull");
        }


      


        [HttpPut]
        [Route("UpdateSubjectById/{id}")]
        public async Task<IActionResult> UpdateSubject(int? id, [FromBody] SubjectsDtoCU SVM)
        {

            if (id == null)
            {
                return BadRequest();
            }

            var subjects = await _unitofwork.Subjects.GetByIdAsync(id);
            if (subjects == null)
            {
                return NotFound();
            }


            subjects.SubjectNameEnglish = SVM.SubjectNameEnglish;
            subjects.SubjectNameArabic = SVM.SubjectNameArabic;
            subjects.SectionId = SVM.SectionId;
            subjects.ScientificDegree = SVM.ScientificDegree;
            subjects.SubjectCode = SVM.SubjectCode;
            subjects.SubjectHours = SVM.SubjectHours;
          

            _unitofwork.SaveChanges();

            return Ok(subjects);
        }



        //for cascading in mvc

        //public async Task<IActionResult> GetSections(int depId) // select to prevent circulation of navigation between models bec i make models refer back and forth
        //{
        //    var section = await _unitofwork.Sections.GetAllAsync();
        //    var filterSection = section.Where(s=>s.DepartmentId==depId).Select(s=> new { s.SectionId,s.SectionName}).ToList();
        //    return Ok(filterSection);

        //}



    }
}
