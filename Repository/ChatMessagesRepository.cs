using FgssrApi.Data;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FgssrApi.Repository
{
    public class ChatMessagesRepository:IChatMessagesRepository
    {

        private readonly ApplicationDbContext _context;

        public ChatMessagesRepository(ApplicationDbContext context) 
        {
            _context = context;  
        }

        public async Task<IEnumerable<ChatMessages>> GetAllAsync()
        {
           var msg = await _context.ChatMessages.Include(m=>m.Sender).OrderBy(m=>m.Timestamp).ToListAsync();
           return msg;
        }


    }
}
