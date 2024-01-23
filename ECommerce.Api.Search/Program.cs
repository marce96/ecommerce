using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();

var ordersServiceConfiguration = builder.Configuration["Services:Orders"]!;
builder.Services.AddHttpClient("OrdersService", config =>
{
    config.BaseAddress = new Uri(ordersServiceConfiguration);
}).AddTransientHttpErrorPolicy(options =>
    options.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

var CustomersServiceConfiguration = builder.Configuration["services:Customers"]!;
builder.Services.AddHttpClient("CustomersService", config =>
{
    config.BaseAddress = new Uri(CustomersServiceConfiguration);
}).AddTransientHttpErrorPolicy(options =>
    options.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

var ProductsServiceConfiguration = builder.Configuration["services:Products"]!;
builder.Services.AddHttpClient("ProductsService", config =>
{
    config.BaseAddress = new Uri(ProductsServiceConfiguration);
}).AddTransientHttpErrorPolicy(options => 
    options.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
