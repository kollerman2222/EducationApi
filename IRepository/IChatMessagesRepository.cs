using FgssrApi.Models;

namespace FgssrApi.IRepository
{
    public interface IChatMessagesRepository
    {
        Task<IEnumerable<ChatMessages>> GetAllAsync();

    }
}
