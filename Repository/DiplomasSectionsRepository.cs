using FgssrApi.Data;
using FgssrApi.Dtos;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FgssrApi.Repository
{
    public class DiplomasSectionsRepository:IDiplomasSectionsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;

        public DiplomasSectionsRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/sections";
        }

        public string uploadFolderPublic { get { return _uploadFolderPath; } }

        public async Task<DiplomasSections> CreateAsync(DiplomasSectionsDtoCU DSVM)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(DSVM.UploadImage?.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await DSVM.UploadImage.CopyToAsync(stream);

            DiplomasSections ds = new()
            {
              SectionName=DSVM.SectionName,
              Description=DSVM.Description,
              DepartmentId=DSVM.DepartmentId,
              SectionImage=imageName
            };
            await _context.DiplomasSections.AddAsync(ds);
            return ds;
        }

        public void Delete(DiplomasSections ds)
        {

            _context.DiplomasSections.Remove(ds);
        }

        public async Task<IEnumerable<DiplomasSections>> GetAllAsync()
        {
            var sections = await _context.DiplomasSections.Include(sec => sec.Department).ToListAsync();
            return sections;
        }

        public async Task<DiplomasSections?> GetByIdAsync(int? id)
        {
            var sections = await _context.DiplomasSections.Include(sec => sec.Department).FirstOrDefaultAsync(sec => sec.SectionId==id);
            return sections;
        }

        public async Task<DiplomasSections?> GetByNameAsync(string name)
        {
            var sections = await _context.DiplomasSections.Include(sec => sec.Department).FirstOrDefaultAsync(sec => sec.SectionName == name);
            return sections;
        }


        public async Task<string?> GetNamebyIdAsync(int? id)
        {
            var sections = await _context.DiplomasSections.Include(sec => sec.Department).FirstOrDefaultAsync(sec => sec.SectionId == id);
            return sections?.SectionName;
        }

        public async Task<string> SaveNewImage(IFormFile image)
        {

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await image.CopyToAsync(stream);

            return imageName;
        }
    }
}
