using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProviders _ordersProvider;

        public OrdersController(IOrdersProviders ordersProvider)
        {
            _ordersProvider = ordersProvider;
        }

        [HttpGet("{id}")]
        public IActionResult GetOrdersByCustomerId(int id)
        {
            var (statusCode, orders, errorMessage) = _ordersProvider.GetOrders(id);
            return statusCode switch
            {
                StatusCodes.Status200OK => Ok(orders),
                _ when string.IsNullOrEmpty(errorMessage) => StatusCode(statusCode),
                _ => StatusCode(statusCode, errorMessage)
            };
        }
    }
}
