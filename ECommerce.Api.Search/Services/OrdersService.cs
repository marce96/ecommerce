using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Newtonsoft.Json;
using System.Net;

namespace ECommerce.Api.Search.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IHttpClientFactory httpClientFactory, ILogger<OrdersService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<(int statusCode, IEnumerable<Order>? orders, string? errorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("OrdersService");
                var response = await client.GetAsync($"api/orders/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<Order>>(content) ?? Enumerable.Empty<Order>();
                    return (((int)response.StatusCode),  result, null);
                }

                return (((int)response.StatusCode), null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrdersAsync {ex}");
                throw;
            }
        }
    }
}
