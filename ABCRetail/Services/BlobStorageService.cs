using Azure.Storage.Blobs;

namespace ABCRetail.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorage");
            string containerName = configuration["BlobContainer"];

            _container = new BlobContainerClient(connectionString, containerName);

            _container.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            BlobClient blobClient = _container.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}