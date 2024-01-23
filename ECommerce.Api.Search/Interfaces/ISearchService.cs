namespace ECommerce.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(int statusCode, dynamic? SearchResults, string? errorMessage)> SearchAsync(int customerId);
    }
}
