using ABCRetail.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class FilesController : Controller
    {
        private readonly FileStorageService _fileStorageService;

        public FilesController(FileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _fileStorageService.GetFilesAsync();

            return View(files);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await using Stream stream = file.OpenReadStream();

                await _fileStorageService.UploadFileAsync(
                    file.FileName,
                    stream);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}