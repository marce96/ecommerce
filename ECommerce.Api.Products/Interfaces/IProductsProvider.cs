using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(int statusCode, IEnumerable<ProductDto> products, string? errorMessage)> GetProductsAsync();
        Task<(int statusCode, ProductDto? product, string? errorMessage)> GetProductAsync(int id);
    }
}
