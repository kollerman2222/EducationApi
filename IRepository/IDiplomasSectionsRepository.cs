using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.ViewModels;

namespace FgssrApi.IRepository
{
    public interface IDiplomasSectionsRepository
    {
        Task<DiplomasSections?> GetByIdAsync(int? id);

        Task<DiplomasSections?> GetByNameAsync(string name);

        Task<IEnumerable<DiplomasSections>> GetAllAsync();

        Task<DiplomasSections> CreateAsync(DiplomasSectionsDtoCU DSVM);


        Task<string?> GetNamebyIdAsync(int? id);

        Task<string> SaveNewImage(IFormFile image);

        string uploadFolderPublic { get; }

        void Delete(DiplomasSections DS);

    }
}
