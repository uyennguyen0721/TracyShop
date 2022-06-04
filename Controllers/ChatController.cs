using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(ILogger<ChatController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/chat")]
        public ActionResult Chat()
        {
            var chats = _context.Chats.Where(c => c.UserId.Contains(_userManager.GetUserId(HttpContext.User))).ToList();

            ViewBag.Start = _context.Messages.FirstOrDefault(m => m.RequestMessage.ToLower().Contains("Bắt đầu")).ResponseMessage;

            if (chats.Count == 0)
            {
                ViewBag.HasChat = false;
                return View();
            }
            else
            {
                ViewBag.HasChat = true;
                List<ChatViewModel> chatViews = new List<ChatViewModel>();
                foreach(var item in chats)
                {
                    var message = _context.Messages.FirstOrDefault(m => m.Id == item.MessageId);
                    var chat = _context.Chats.FirstOrDefault(c => c.Id == item.Id);

                    List<ResponseMessageViewModel> responses = new List<ResponseMessageViewModel>();

                    // Thêm response message từ chatbot
                    responses.Add(new ResponseMessageViewModel
                    {
                        CreatedAt = chat.CreatedAt,
                        Message = message.ResponseMessage
                    });

                    // kiểm tra xem có response trực tiếp từ admin hay không (dữ liệu trong bảng ResponseMessage)
                    // Nếu có thì add vào danh sách các responses
                    var responseMessages = _context.ResponseMessages.Where(r => r.ChatId == chat.Id).ToList();
                    if (responseMessages.Count > 0)
                    {
                        foreach(var res in responseMessages)
                        {
                            responses.Add(new ResponseMessageViewModel
                            {
                                CreatedAt = res.CreatedAt,
                                Message = res.Response
                            });
                        }
                    }

                    var chatView = new ChatViewModel
                    {
                        CreatedAt = item.CreatedAt,
                        UserName = _context.Users.FirstOrDefault(u => u.Id.Contains(item.UserId)).UserName,
                        Request = chat.Request,
                        Responses = responses
                    };
                    chatViews.Add(chatView);

                }
                ViewBag.Chat = chatViews;
                return View();
            }  
        }

        [Authorize]
        [HttpPost("/chat")]
        public async Task<IActionResult> Chat(RequestMessageViewModel request)
        {
            var message = await _context.Messages
                .FirstOrDefaultAsync(m => request.Message.ToLower().Contains(m.RequestMessage.ToLower()));

            if(message != default)
            {
                var chat = new Chat()
                {
                    UserId = _userManager.GetUserId(HttpContext.User),
                    MessageId = message.Id,
                    Request = request.Message
                };

                await _context.Chats.AddAsync(chat);
            }
            else
            {
                var other = await _context.Messages
                .FirstOrDefaultAsync(m => m.RequestMessage.ToLower().Contains("Khác"));

                var chat = new Chat()
                {
                    UserId = _userManager.GetUserId(HttpContext.User),
                    MessageId = other.Id,
                    Request = request.Message
                };

                await _context.Chats.AddAsync(chat);
            }

            var result = await _context.SaveChangesAsync();

            if (result == 1)
            {
                ViewBag.Message = "";
            }
            else
            {
                ViewBag.Message = "Hệ thống đang xảy ra sự cố, vui lòng thử lại.";
            }

            return RedirectToAction(nameof(Chat));
        }
    }
}
