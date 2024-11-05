using FgssrApi.Dtos;
using FgssrApi.Models;
using FgssrApi.ViewModels;

namespace FgssrApi.IRepository
{
    public interface INewsRepository
    {
        Task<News?> GetByIdAsync(int? id);

        Task<IEnumerable<News>> GetAllAsync();

        Task<News> CreateAsync(NewsDtoCU NVM);

        void Delete(News news);
    }
}
