using Azure.Storage.Queues;

namespace ABCRetail.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration.GetConnectionString("AzureStorage")
                ?? throw new InvalidOperationException(
                    "AzureStorage connection string is not configured.");

            _queueClient = new QueueClient(
                connectionString,
                "order-processing");

            _queueClient.CreateIfNotExists();
        }

        public async Task AddMessageAsync(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _queueClient.SendMessageAsync(message);
            }
        }

        public async Task<List<string>> GetMessagesAsync()
        {
            var messages = new List<string>();

            var response = await _queueClient.ReceiveMessagesAsync(32);

            foreach (var message in response.Value)
            {
                messages.Add(message.MessageText);
            }

            return messages;
        }
    }
}