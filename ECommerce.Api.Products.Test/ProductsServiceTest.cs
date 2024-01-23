using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Test
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProductsAsync()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnsAllProductsAsync)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(c => c.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProviders(dbContext, null, mapper);

            var products = await productsProvider.GetProductsAsync();
            Assert.True(products.statusCode == 200);
            Assert.True(products.products.Any());
            Assert.Null(products.errorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingValidIdAsync()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsProductUsingValidIdAsync)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(c => c.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProviders(dbContext, null, mapper);

            int productId = 1;
            var products = await productsProvider.GetProductAsync(productId);
            Assert.True(products.statusCode == 200);
            Assert.NotNull(products.product);
            Assert.Null(products.errorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingInvalidIdAsync()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsProductUsingInvalidIdAsync)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(c => c.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProviders(dbContext, null, mapper);

            int productId = 10;
            var products = await productsProvider.GetProductAsync(productId);
            Assert.True(products.statusCode == StatusCodes.Status404NotFound);
            Assert.Null(products.product);
        }

        private static void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 0; i < 3; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i + 1,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });

                dbContext.SaveChanges();
            }
        }
    }
}