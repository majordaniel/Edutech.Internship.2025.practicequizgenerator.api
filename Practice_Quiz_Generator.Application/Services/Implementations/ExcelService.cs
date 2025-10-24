using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Data;
using System.Text;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        private readonly IWebHostEnvironment _environment;

        public ExcelService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task<ExcelResponseDto> ExportDataAsync<T>(string fileName, IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }

        public async Task<DataTable> ExtractFileDataAsync(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });

            return result.Tables[0];
        }

        public ExcelResponseDto GetTemplate(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
