using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.ViewModels;

namespace FgssrApi.IRepository
{
    public interface ISubjectsRepository
    {
        Task<Subjects?> GetByIdAsync(int? id);

        Task<IEnumerable<Subjects>> GetAllAsync();

        Task<IEnumerable<Subjects>> GetAllBySectionNameAsync(string sectionName);

        Task<Subjects> CreateAsync(SubjectsDtoCU SVM);

        void Delete(Subjects SVM);

    }
}
