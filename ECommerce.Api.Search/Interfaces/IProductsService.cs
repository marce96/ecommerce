using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IProductsService
    {
        Task<(int statusCode, IEnumerable<Product>? products, string? errorMessage)> GetProductsAsync();
    }
}
