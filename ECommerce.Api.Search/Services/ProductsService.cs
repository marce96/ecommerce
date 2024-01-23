using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Newtonsoft.Json;

namespace ECommerce.Api.Search.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IHttpClientFactory httpClientFactory, ILogger<ProductsService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<(int statusCode, IEnumerable<Product>? products, string? errorMessage)> GetProductsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ProductsService");
                var response = await client.GetAsync("api/products");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);
                    return ((int)response.StatusCode, result, null);
                }

                return ((int)response.StatusCode, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductsAsync {ex}");
                return (500, null, ex.ToString());
            }
        }
    }
}
