using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableStorageService _tableStorageService;
        private readonly BlobStorageService _blobStorageService;

        public ProductsController(
            TableStorageService tableStorageService,
            BlobStorageService blobStorageService)
        {
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
        }

        // Display all products
        public IActionResult Index()
        {
            var products = _tableStorageService.GetProducts();
            return View(products);
        }

        // Display Create page
        public IActionResult Create()
        {
            return View();
        }

        // Save Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductEntity product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    product.ImageUrl = await _blobStorageService.UploadFileAsync(imageFile);
                }

                await _tableStorageService.AddProductAsync(product);

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
    }
}