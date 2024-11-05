using FgssrApi.UnitOFWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FgssrApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesManagmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;

        public FilesManagmentController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        [HttpGet]
        [Route("GetAllUsersFolders")]
        public IActionResult GetAllUsersFolders()
        {
            var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/gradcerts");

            var foldersNames = Directory.GetDirectories(mainPath).Select(Path.GetFileName).ToList();

            return Ok(foldersNames);
        }


        [HttpGet]
        [Route("GetSpecificUserFiles/{userfolderName}")]
        public IActionResult specificUserFiles(string userfolderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/gradcerts", userfolderName);

            var files = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();

            return Ok(files);
        }
    }
}
