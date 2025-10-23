using Microsoft.AspNetCore.Http;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Data;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IExcelService
    {
        Task<DataTable> ExtractFileDataAsync(IFormFile file);
        Task<ExcelResponseDto> ExportDataAsync<T>(string fileName, IEnumerable<T> data);
        ExcelResponseDto GetTemplate(string fileName);
    }
}
