using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class FileProcessingService : IFileProcessingService
    {
        public async Task<string> ExtractTextAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var extension = Path.GetExtension(file.FileName).ToLower();

            switch (extension)
            {
                case ".txt":
                    using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                    {
                        return await reader.ReadToEndAsync();
                    }

                case ".pdf":
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        var pdfText = new StringBuilder();
                        using (var document = PdfDocument.Open(ms.ToArray()))
                        {
                            foreach (var page in document.GetPages())
                                pdfText.AppendLine(page.Text);
                        }
                        return pdfText.ToString();
                    }

                case ".docx":
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        using (var wordDoc = WordprocessingDocument.Open(ms, false))
                        {
                            var body = wordDoc.MainDocumentPart.Document.Body;
                            return body.InnerText;
                        }
                    }

                default:
                    throw new NotSupportedException($"File format {extension} not supported");
            }
        }
    }
}
