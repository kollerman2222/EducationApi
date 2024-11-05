using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using System.Linq.Expressions;

namespace FgssrApi.IRepository
{
    public interface IDiplomasDepartmentsRepository
    {
        Task<DiplomasDepartments?> GetByIdAsync(int? id);

        Task<IEnumerable<DiplomasDepartments>> GetAllAsync();

        Task<DiplomasDepartments> CreateAsync(DiplomasDepartmentsDtoCU DDVM);

        //Task UpdateAsync(DiplomasDepartmentsViewModel DDVM);

        void Delete(DiplomasDepartments dep);

        

    }
}
