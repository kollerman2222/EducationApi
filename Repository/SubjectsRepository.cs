using FgssrApi.Data;
using FgssrApi.Dtos;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;

namespace FgssrApi.Repository
{
    public class SubjectsRepository:ISubjectsRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Subjects> CreateAsync(SubjectsDtoCU SVM)
        {

            Subjects ss = new()
            {
                SubjectNameArabic = SVM.SubjectNameArabic,
                SubjectNameEnglish = SVM.SubjectNameEnglish,
                ScientificDegree = SVM.ScientificDegree,
                SubjectCode = SVM.SubjectCode,
                SubjectHours = SVM.SubjectHours,
                SubjectSemester = SVM.SubjectSemester,
                SectionId=SVM.SectionId

            };
            await _context.Subjects.AddAsync(ss);
            return ss;
        }


        public void Delete(Subjects ss)
        {

            _context.Subjects.Remove(ss);
        }


        public async Task<IEnumerable<Subjects>> GetAllAsync()
        {
            var subjects = await _context.Subjects.Include(ss => ss.Section.Department).ToListAsync();
            return subjects;
        }


        public async Task<IEnumerable<Subjects>> GetAllBySectionNameAsync(string sectionName)
        {
            var subjects = await _context.Subjects.Where(ss => ss.Section.SectionName==sectionName).ToListAsync();
            return subjects;
        }


        public async Task<Subjects?> GetByIdAsync(int? id)
        {
            var subjects = await _context.Subjects.Include(ss => ss.Section.Department).FirstOrDefaultAsync(ss => ss.SubjectId == id);
            return subjects;
        }




    }
}
