using FgssrApi.Models;
using FgssrApi.ViewModels;

namespace FgssrApi.IRepository
{
    public interface ISubjectsMarksRepository
    {
        Task CreateAsync(SubjectsMarksViewModel SMVM);

        Task<IEnumerable<SubjectMark>> GetAllByUserIdAsync(string? id);

        void Delete(SubjectMark SM);
    }
}
