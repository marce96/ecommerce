using ECommerce.Api.Orders.Models;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrdersProviders
    {
        (int statusCode, IEnumerable<OrderDto> orders, string? errorMessage) GetOrders(int customerId);
    }
}
