using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class OrdersController : Controller
    {
        private readonly QueueStorageService _queueStorageService;

        public OrdersController(QueueStorageService queueStorageService)
        {
            _queueStorageService = queueStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _queueStorageService.GetMessagesAsync();

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _queueStorageService.AddMessageAsync(message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}