using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using SmartLeave.MVC.Models;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
namespace SmartLeave.MVC.Controllers
{

    public class ChatController : Controller
    {
        private readonly OpenAIChatService _chatService;

        public ChatController(OpenAIChatService chatService)
        {
            _chatService = chatService;
        }

        private static List<ChatMessage> _chatHistory = new()
    {
        new ChatMessage(ChatRole.System, """
        You are a helpful SmartLeave assistant. 
        You help employees by answering questions about:
        - Leave balance
        - Leave requests
        - Public holidays
        - Suggestions for time off

        Always greet the user, and be professional and concise.
        """)
    };

        [HttpGet]
        public IActionResult Index() => View();


        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            _chatHistory.Add(new ChatMessage(ChatRole.User, request.Prompt));
            var response = await _chatService.GetResponseAsync(_chatHistory);
            _chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
            return Json(new { response });
        }

    }

}