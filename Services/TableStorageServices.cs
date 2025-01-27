using Azure.Data.Tables;
using Azure;
using CLDV_POE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CLDV_POE.Services
{
    public class TableStorageServices
    {
        private readonly TableClient _productTableClient; //For Product Table Storage
        private readonly TableClient _userTableClient; //For Blob Storage
        private readonly TableClient _processTableClient; //Used for Queue Storage

        public TableStorageServices(string connectionString)
        {
            _productTableClient = new TableClient(connectionString, "Product");
            _userTableClient = new TableClient(connectionString, "User");
            _processTableClient = new TableClient(connectionString, "Process");
        }



        // Products Methods
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            await foreach (var product in _productTableClient.QueryAsync<Product>())
            {
                products.Add(product);
            }
            return products;

        }



        public async Task addProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.PartitionKey) || string.IsNullOrEmpty(product.RowKey))
            {
                throw new ArgumentException("Partition key must be set.");
            }

            try
            {
                await _productTableClient.AddEntityAsync(product);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to table storage", ex);
            }
        }

        public async Task DeleteProductAsync(string partitionKey, string rowKey)
        {
            await _productTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<Product?> GetProductAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _productTableClient.GetEntityAsync<Product>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Handles not found
                return null;
            }

        }


        // User Methods
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            await foreach (var user in _userTableClient.QueryAsync<User>())
            {
                users.Add(user);
            }
            return users;

        }

        public async Task addUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.PartitionKey) || string.IsNullOrEmpty(user.RowKey))
            {
                throw new ArgumentException("Partition key must be set.");
            }

            try
            {
                await _userTableClient.AddEntityAsync(user);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to table storage", ex);
            }
        }

        public async Task DeleteUserAsync(string partitionKey, string rowKey)
        {
            await _userTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<User?> GetUserAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _userTableClient.GetEntityAsync<User>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Handles not found
                return null;
            }

        }

        public async Task AddProcessAsync(Process process)
        {
            if (string.IsNullOrEmpty(process.PartitionKey) || string.IsNullOrEmpty(process.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set");
            }
            try
            {
                await _processTableClient.AddEntityAsync(process);

            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding process to" + "table storage", ex);
            }
        }

        public async Task<List<Process>> GetAllProcessAsync()
        {
            var processes = new List<Process>();
            await foreach (var process in _processTableClient.QueryAsync<Process>())
            {
                processes.Add(process);
            }
            return processes;
        }

        public async Task DeleteProcessAsync(string partitionKey, string rowKey)
        {
            try
            {
                await _processTableClient.DeleteEntityAsync(partitionKey, rowKey);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                throw new KeyNotFoundException("The process entity was not found.", ex);
            }

        }
    }
}

