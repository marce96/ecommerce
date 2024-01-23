using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProviders : IOrdersProviders
    {
        private readonly OrdersDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersProviders> _logger;

        public OrdersProviders(OrdersDbContext context, IMapper mapper, ILogger<OrdersProviders> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;

            Task.FromResult(SeedDataAsync());
        }

        private async Task SeedDataAsync()
        {
            var hasData = _context.Orders.Any();
     
            if(!hasData)
            {
                var newOrder = new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Items = new List<OrderItem>()
                         {
                             new() {
                                 Id = 1,
                                 OrderId = 1,
                                 ProductId = 1,
                                 UnitPrice = 20,
                                 Quantity = 2
                             },
                             new()
                             {
                                 Id = 2,
                                 OrderId = 1,
                                 ProductId = 2,
                                 UnitPrice = 120,
                                 Quantity = 2
                             }
                         },
                };

                newOrder.Total = newOrder.Items.Sum(o => o.UnitPrice);

                await _context.Orders.AddAsync(newOrder);

                await _context.SaveChangesAsync();
            };
        }
        public (int statusCode, IEnumerable<OrderDto> orders, string? errorMessage) GetOrders(int customerId)
        {
            try
            {
                var orders = _context.Orders.Where(x => x.CustomerId == customerId).Include(i => i.Items).ToImmutableList();
                var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return (StatusCodes.Status200OK, ordersDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrdersAsync {ex}");
                throw;
            }
        }
    }
}
