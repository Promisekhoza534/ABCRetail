using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetail.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration.GetConnectionString("AzureStorage")
                ?? throw new InvalidOperationException(
                    "AzureStorage connection string is not configured.");

            _shareClient = new ShareClient(
                connectionString,
                "application-logs");

            _shareClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(
            string fileName,
            Stream fileStream)
        {
            ShareDirectoryClient directoryClient =
                _shareClient.GetRootDirectoryClient();

            ShareFileClient fileClient =
                directoryClient.GetFileClient(fileName);

            await fileClient.CreateAsync(fileStream.Length);

            await fileClient.UploadAsync(fileStream);
        }

        public async Task<List<string>> GetFilesAsync()
        {
            var files = new List<string>();

            ShareDirectoryClient directoryClient =
                _shareClient.GetRootDirectoryClient();

            await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                {
                    files.Add(item.Name);
                }
            }

            return files;
        }
    }
}