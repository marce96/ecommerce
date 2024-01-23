using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        (int statusCode, IEnumerable<CustomerDto> costumers, string? errorMessage) GetCustomers();
        Task<(int statusCode, CustomerDto? costumer, string? errorMessage)> GetCustomerAsync(int id);
    }
}
