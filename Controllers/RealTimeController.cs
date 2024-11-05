//using FgssrApi.Data;
//using FgssrApi.Hubs;
//using FgssrApi.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;

//namespace FgssrApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RealTimeController : ControllerBase
//    {
//        private IHubContext<MessageHub, IMessageHub> _messageHub;
//        private readonly ApplicationDbContext _context;
//        private readonly IHubContext<ChatHub> _hubContext;


//        public RealTimeController(IHubContext<MessageHub, IMessageHub> messageHub, ApplicationDbContext context, IHubContext<ChatHub> hubContext)
//        {
//            _messageHub = messageHub;
//            _context = context;
//            _hubContext = hubContext;
//        }




//        [HttpGet("send-message")]
//        public async Task<IActionResult> SendMessage([FromQuery] string user, [FromQuery] string message)
//        {
//            var msg = new ChatMessages
//            {
//                Content = message,
//                SenderId = "b3e76658-c0ac-401c-a0eb-e6eb8cf8b47c",
//                SenderName = user,
//                ProfileImage = "NoImage"

//            };
//            _context.ChatMessages.Add(msg);
//            _context.SaveChanges();
//            await _messageHub.Clients.All.SendMessageToUsers(user, message);
//            return Ok("Message sent.");
//        }


//        [HttpGet("SendChatMessage")]
//        public async Task<IActionResult> SendChatMessage([FromQuery] string user, [FromQuery] string message)
//        {
//            var msg = new ChatMessages
//            {
//                Content = message,
//                SenderId = "b3e76658-c0ac-401c-a0eb-e6eb8cf8b47c",
//                SenderName = user,
//                ProfileImage = "NoImage2"

//            };
//            _context.ChatMessages.Add(msg);
//            _context.SaveChanges();
//            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
//            return Ok("New Message sent");
//        }

//    }
//}
