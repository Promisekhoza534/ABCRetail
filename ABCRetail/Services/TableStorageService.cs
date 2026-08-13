using Azure.Data.Tables;
using ABCRetail.Models;

namespace ABCRetail.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customersTable;
        private readonly TableClient _productsTable;

        public TableStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorage");

            _customersTable = new TableClient(connectionString, "Customers");
            _productsTable = new TableClient(connectionString, "Products");

            _customersTable.CreateIfNotExists();
            _productsTable.CreateIfNotExists();
        }

        // -------------------- Customers --------------------

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            await _customersTable.AddEntityAsync(customer);
        }

        public List<CustomerEntity> GetCustomers()
        {
            return _customersTable.Query<CustomerEntity>().ToList();
        }

        // -------------------- Products --------------------

        public async Task AddProductAsync(ProductEntity product)
        {
            await _productsTable.AddEntityAsync(product);
        }

        public List<ProductEntity> GetProducts()
        {
            return _productsTable.Query<ProductEntity>().ToList();
        }
    }
}