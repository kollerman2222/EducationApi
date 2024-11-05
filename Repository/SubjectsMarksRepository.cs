using FgssrApi.Data;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace FgssrApi.Repository
{
    public class SubjectsMarksRepository : ISubjectsMarksRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectsMarksRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(SubjectsMarksViewModel SMVM)
        {
            SubjectMark SM = new()
            {
                StudentId = SMVM.StudentId,
                SubjectId = SMVM.SubjectId,
                Grade = SMVM.Grade,
                Percentage = SMVM.Percentage,
            };

           await _context.SubjectMark.AddAsync(SM);
        }

        public void Delete(SubjectMark SM)
        {
            _context.SubjectMark.Remove(SM);
        }

        public async Task<IEnumerable<SubjectMark>> GetAllByUserIdAsync(string? id)
        {
            var sM = await _context.SubjectMark.Where(s => s.StudentId == id).ToListAsync();
            return sM;
        }
    }
}
