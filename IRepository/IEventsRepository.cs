using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.ViewModels;

namespace FgssrApi.IRepository
{
    public interface IEventsRepository
    {

        Task<Events?> GetByIdAsync(int? id);

        Task<IEnumerable<Events>> GetAllAsync();

        Task<Events> CreateAsync(EventsDtoCU EVM);

        Task<string> SaveNewImage(IFormFile image);

        string uploadFolderPublic { get; }

        void Delete(Events eve);
    }
}
