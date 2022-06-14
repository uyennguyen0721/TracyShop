using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/create-message-bot")]
        public IActionResult CreateMessageBot()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/create-message-bot")]
        public async Task<IActionResult> CreateMessageBot(Message message)
        {
            if (ModelState.IsValid)
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/edit-message-bot/{id}")]
        public async Task<IActionResult> EditMessageBot(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        [HttpPost("/edit-message-bot/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMessageBot(int id, Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet]
        public async Task<IActionResult> Chat(string userId)
        {
            if (_context.Chats.ToList().Count == 0)
            {
                ViewBag.Message = "Chưa có cuộc trò chuyện nào.";
                return View();
            }
            else
            {
                ViewBag.Message = "";
                ViewBag.Start = _context.Messages.FirstOrDefault(m => m.RequestMessage.ToLower().Contains("Bắt đầu")).ResponseMessage;
                ViewBag.Users = await GetUsers();

                List<ChatViewModel> chatViews = new List<ChatViewModel>();

                if (userId != null)
                {

                    // cập nhật lại trạng thái của tin nhắn thành đã xem
                    var messages = _context.Chats.Where(m => m.IsSeen == false && m.UserId.Contains(userId)).ToList();

                    ViewBag.User = await GetUser(userId);

                    if(messages.Count > 0)
                    {
                        await SeenMessages(messages);
                    }

                    // lấy các tin nhắn theo user
                    chatViews = GetMessagesByUser(userId);
                }
                else
                {
                    // lấy id của user gửi tin nhắn gần nhất
                    var lastUserId = _context.Chats.OrderByDescending(c => c.CreatedAt).First().UserId;

                    // cập nhật lại trạng thái của tin nhắn thành đã xem
                    var messages = _context.Chats.Where(m => m.IsSeen == false && m.UserId.Contains(lastUserId)).ToList();
                    if (messages.Count > 0)
                    {
                        await SeenMessages(messages);
                    }

                    ViewBag.User = await GetUser(lastUserId);
                    chatViews = GetMessagesByUser(lastUserId);
                }

                ViewBag.Chat = chatViews;

                return View();
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public async Task<IActionResult> Chat(ResponseViewModel responseMessage)
        {
            var chat = _context.Chats.OrderBy(c => c.CreatedAt).LastOrDefault(c => c.UserId.Contains(responseMessage.UserId));

            if(chat == default)
            {
                return NotFound();
            }
            else
            {
                var response = new ResponseMessage
                {
                    Response = responseMessage.Response,
                    ChatId = chat.Id
                };

                await _context.ResponseMessages.AddAsync(response);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Chat));
            }
        }


        #region Helpers

        public async Task<List<UserChatViewModel>> GetUsers()
        {
            List<UserChatViewModel> userChats = new List<UserChatViewModel>();
            if (_context.Chats.ToList().Count != 0)
            {
                var users = _context.Chats.OrderByDescending(c => c.CreatedAt).Select(c => c.UserId).Distinct().ToList();

                foreach (var item in users)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Contains(item));
                    
                    userChats.Add(new UserChatViewModel
                    {
                        Id = user.Id,
                        Avatar = user.Avatar,
                        UserName = user.Name,
                        IsSeen = _context.Chats.FirstOrDefault(c => c.UserId.Contains(user.Id)).IsSeen,
                        LastMessage = _context.Chats.OrderByDescending(c => c.CreatedAt).FirstOrDefault(c => c.UserId.Contains(user.Id)).Request
                    });
                }
            }

            return userChats;
        }

        public async Task<UserChatViewModel> GetUser(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Contains(userId));

            var userChat = new UserChatViewModel
            {
                Id = user.Id,
                Avatar = user.Avatar,
                UserName = user.Name
            };

            return userChat;
        }

        public List<ChatViewModel> GetMessagesByUser(string userId)
        {
            var chats = _context.Chats.Where(c => c.UserId.Contains(userId)).OrderBy(c => c.CreatedAt).ToList();
            List<ChatViewModel> chatViews = new List<ChatViewModel>();

            foreach (var item in chats)
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
                    foreach (var res in responseMessages)
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
                    UserId = userId,
                    Request = chat.Request,
                    Responses = responses
                };
                chatViews.Add(chatView);
            }

            return chatViews;
        }

        public async Task SeenMessages(List<Chat> messages)
        {
            if(messages.Count > 0)
            {
                foreach(var item in messages)
                {
                    item.IsSeen = true;
                    _context.Chats.Update(item);
                }
                await _context.SaveChangesAsync();
            }
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }

        #endregion
    }
}
