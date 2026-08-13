using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Services;

namespace ABCRetail.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public CustomersController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        // Display all customers
        public IActionResult Index()
        {
            var customers = _tableStorageService.GetCustomers();
            return View(customers);
        }

        // Show create form
        public IActionResult Create()
        {
            return View();
        }

        // Save customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                await _tableStorageService.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }
    }
}