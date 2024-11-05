using FgssrApi.Data;
using FgssrApi.Dtos;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FgssrApi.Repository
{
    public class NewsRepository:INewsRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<News> CreateAsync(NewsDtoCU NVM)
        {

            News nn = new()
            {
              NewsDescription=NVM.NewsDescription,
              NewsTitle=NVM.NewsTitle,
            };
            await _context.News.AddAsync(nn);
            return nn;
        }

        public void Delete(News news)
        {

            _context.News.Remove(news);
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            var news = await _context.News.ToListAsync();
            return news;
        }

        public async Task<News?> GetByIdAsync(int? id)
        {
            var news = await _context.News.FindAsync(id);

            return news;
        }

    }
}
