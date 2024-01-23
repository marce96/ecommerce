using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Newtonsoft.Json;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(IHttpClientFactory clientFactory, ILogger<CustomersService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        public async Task<(int statusCode, Customer? customer, string? errorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient("CustomersService");
                var response = await httpClient.GetAsync($"api/customers/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Customer>(content);
                    return ((int)response.StatusCode, result, null);
                }
                return ((int) response.StatusCode, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (500, null, ex.ToString());
            }
        }

        public async Task<(int statusCode, IEnumerable<Customer> customers, string? errorMessage)> GetCustomersAsync()
        {
            try
            {
                var httpClient = _clientFactory.CreateClient("CustomersService");
                var response = await httpClient.GetAsync("api/customers");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<Customer>>(content) ?? Enumerable.Empty<Customer>();
                    return ((int)response.StatusCode, result, null);
                }
                return ((int)response.StatusCode, Enumerable.Empty<Customer>(), response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (500, Enumerable.Empty<Customer>(), ex.ToString());
            }
        }
    }
}
