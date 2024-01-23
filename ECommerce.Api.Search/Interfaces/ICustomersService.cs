using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface ICustomersService
    {
        Task<(int statusCode, IEnumerable<Customer> customers, string? errorMessage)> GetCustomersAsync();
        Task<(int statusCode, Customer? customer, string? errorMessage)> GetCustomerAsync(int customerId);
    }
}
