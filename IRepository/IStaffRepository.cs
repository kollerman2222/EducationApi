using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.ViewModels;

namespace FgssrApi.IRepository
{
    public interface IStaffRepository
    {
        Task<Staff?> GetByIdAsync(int? id);

        Task<IEnumerable<Staff>> GetAllAsync();

        Task<Staff> CreateAsync(StaffsDtoCU SVM);

        Task<string> SaveNewImage(IFormFile image);

        string uploadFolderPublic { get; }

        void Delete(Staff staff);
    }
}
