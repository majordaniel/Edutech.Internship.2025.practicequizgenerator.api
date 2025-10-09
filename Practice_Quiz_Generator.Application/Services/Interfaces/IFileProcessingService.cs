using Microsoft.AspNetCore.Http;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IFileProcessingService
    {
        Task<string> ExtractTextAsync(IFormFile file);
    }
}
