using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Search.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(SearchTermDto searchTerm)
        {
            var (statusCode, searchResults, errorMessage) = await _searchService.SearchAsync(searchTerm.CustomerId);
            return statusCode switch
            {
                StatusCodes.Status200OK => Ok(searchResults),
                _ when !string.IsNullOrEmpty(errorMessage) => StatusCode(statusCode, errorMessage),
                _ => StatusCode(statusCode)
            };
        }
    }
}
