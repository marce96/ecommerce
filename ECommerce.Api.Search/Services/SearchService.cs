using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        private readonly ICustomersService _customersService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            _ordersService = ordersService;
            _productsService = productsService;
            _customersService = customersService;
        }
        public async Task<(int statusCode, dynamic? SearchResults, string? errorMessage)> SearchAsync(int customerId)
        {
            var (statusCode, orders, errorMessage) = await _ordersService.GetOrdersAsync(customerId);
            var customerResult = await _customersService.GetCustomerAsync(customerId);

            var ordersExists = orders != null && orders.Any();
            if(ordersExists)
            {
                var products = await _productsService.GetProductsAsync();
                foreach (var order in orders!)
                { 
                    foreach (var item in order.Items)
                    {
                        item.ProductName = products.products?.FirstOrDefault(x => x.Id == item.ProductId)?.Name ?? "Product information is not available";
                    }
                }
            }
            var result = new
            {
                CustomerName = customerResult.customer?.Name ?? "Customer Information is not availble",
                Orders = orders
            };

            return (statusCode, result, errorMessage);
        }
    }
}
