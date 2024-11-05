using FgssrApi.Controllers;
using FgssrApi.Data;
using FgssrApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FgssrApi.Hubs
{
    public class ChatHub:Hub
    {

        // for web api this class can be empty 
        // we can also instead of doing everything in controller ( broadcasting and saving to database ) 
        // we can make this class handle the broadcasting normally and on user click silently ajax request hit an endpoint to do the database saving 

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public ChatHub(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(string currentUser, string message)
        {
            var id = "b3e76658-c0ac-401c-a0eb-e6eb8cf8b47c";
            var getUser = await _userManager.FindByNameAsync(currentUser);
            var user = getUser?.UserName;
            var profileImage = getUser?.ProfileImage;
            var msg = new ChatMessages
            {
                Content = message,
                SenderId = getUser?.Id,
                SenderName = currentUser,
                ProfileImage=profileImage
                
            };
            _context.ChatMessages.Add(msg);
            _context.SaveChanges();
            await Clients.All.SendAsync("ReceiveMessage",user, message,profileImage);
        }

        public Task<string> GetStaticMessage()
        {
            return Task.FromResult("Hello from SignalR Hub!");
        }


    }
}
