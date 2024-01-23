using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IOrdersService
    {
        Task<(int statusCode, IEnumerable<Order>? orders, string? errorMessage)> GetOrdersAsync(int customerId);
    }
}
