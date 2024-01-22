using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProviders : ICustomersProvider
    {
        private readonly CustomersDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerProviders> _logger;

        public CustomerProviders(CustomersDbContext context, IMapper mapper, ILogger<CustomerProviders> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;

            Task.FromResult(SeedDataAsync());
        }

        private async Task SeedDataAsync()
        {
            var hasData = _context.Customers.Any();
            if (!hasData)
            {
                await _context.Customers.AddRangeAsync(
                   new Customer { Id = 1, Name = "Carmen", Address = "address 1" },
                   new Customer { Id = 2, Name = "Marcela", Address = "address 2" },
                   new Customer { Id = 3, Name = "Mike", Address = "address 3" }
                  );
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(int statusCode, CustomerDto? costumer, string? errorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

                if (customer == null)
                    return (StatusCodes.Status404NotFound, null, null);

                var customerDto = _mapper.Map<CustomerDto>(customer);
                return (StatusCodes.Status200OK, customerDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCustomerAsync {ex}");
                throw;
            }
        }

        public (int statusCode, IEnumerable<CustomerDto> costumers, string? errorMessage) GetCustomers()
        {
            try
            {
                var customers =  _context.Customers.ToImmutableList();
                var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
                return (StatusCodes.Status200OK, customersDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCustomers {ex}");
                throw;
            }
        }
    }
}
