using FgssrApi.Data;
using FgssrApi.Dtos;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FgssrApi.Repository
{
    public class DiplomasDepartmentsRepository : IDiplomasDepartmentsRepository
    {
        private readonly ApplicationDbContext _context;

        public DiplomasDepartmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiplomasDepartments> CreateAsync(DiplomasDepartmentsDtoCU DDVM)
        {

            DiplomasDepartments dd = new()
            {
               DepartmentName = DDVM.DepartmentName,
               Description = DDVM.Description,

            };
             await _context.DiplomasDepartments.AddAsync(dd);
            return dd;
        }

        public void Delete(DiplomasDepartments dep)
        {
                 
             _context.DiplomasDepartments.Remove(dep);        
        }

        public async Task<IEnumerable<DiplomasDepartments>> GetAllAsync()
        {
            var departments = await _context.DiplomasDepartments.Include(dep => dep.Sections).ToListAsync();
            return departments;
        }

        public async Task<DiplomasDepartments?> GetByIdAsync(int? id)
        {
            var departments = await _context.DiplomasDepartments.Include(dep => dep.Sections).FirstOrDefaultAsync(d=>d.DepartmentId == id);
           
           return departments;                    
        }

        //public async Task UpdateAsync(DiplomasDepartmentsViewModel DDVM)
        //{

        //    var departments = await _context.DiplomasDepartments.FindAsync(DDVM.DepId);

        //    departments.DepartmentName = DDVM.DepartmentName;
        //    departments.Description = DDVM.Description;            
        //}
    }
}
