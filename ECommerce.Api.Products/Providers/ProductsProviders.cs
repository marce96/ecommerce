using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProviders : IProductsProvider
    {
        private readonly ProductsDbContext _context;
        private readonly ILogger<ProductsProviders> _logger;
        private readonly IMapper _mapper;
        public ProductsProviders(ProductsDbContext context, ILogger<ProductsProviders> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;

            Task.FromResult(SeedDataAsync());
        }

        private async Task SeedDataAsync()
        {
            try
            {
                if (!_context.Products.Any())
                {
                    await _context.Products.AddRangeAsync(
                             new Db.Product() { Id = 1, Name = "Keyboard", Price = 70, Inventory = 15 },
                             new Db.Product() { Id = 2, Name = "Mouse", Price = 30, Inventory = 8 },
                             new Db.Product() { Id = 3, Name = "Monitor", Price = 180, Inventory = 22 },
                             new Db.Product() { Id = 4, Name = "CPU", Price = 100, Inventory = 18 }
                     );
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SeedDataAsync {ex.ToString}");
                throw;
            }
        }

        public async Task<(int statusCode, IEnumerable<ProductDto> products, string? errorMessage)> GetProductsAsync()
        {
            var productsDto = Enumerable.Empty<ProductDto>();

            try
            {
                var products = await _context.Products.ToListAsync();
                productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
                return (StatusCodes.Status200OK, productsDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductsAsync {ex.ToString()}");
                return (StatusCodes.Status500InternalServerError, productsDto, ex.Message);
            }
        }

        public async Task<(int statusCode, ProductDto? product, string? errorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product == null) 
                    return (StatusCodes.Status404NotFound, null, null);

                var productDto = _mapper.Map<ProductDto>(product);

                return (StatusCodes.Status200OK, productDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductsAsync {ex.ToString()}");
                throw;
            }
        }
    }
}
