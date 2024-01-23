using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Customers.Controllers
{
    [Route("api/customers")]
    [ApiController]

    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider _customersProvider;

        public CustomersController(ICustomersProvider customersProvider)
        {
            _customersProvider = customersProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var (statusCode, customer, errorMessage) = await _customersProvider.GetCustomerAsync(id);

            return statusCode switch
            {
                StatusCodes.Status200OK => Ok(customer),
                _ when string.IsNullOrEmpty(errorMessage) => StatusCode(statusCode),
                _ => StatusCode(statusCode, errorMessage)
            };
        }

        [HttpGet()]
        public IActionResult GetCustomers()
        {
            var (statusCode, customers, errorMessage) = _customersProvider.GetCustomers();

            return statusCode switch
            {
                StatusCodes.Status200OK => Ok(customers),
                _ when string.IsNullOrEmpty(errorMessage) => StatusCode(statusCode),
                _ => StatusCode(statusCode, errorMessage)
            };
        }
    }
}
