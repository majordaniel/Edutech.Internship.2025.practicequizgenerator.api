using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuestionImportService
    {
        Task<Guid> StartImportJobAsync(IFormFile file, string courseId, string adminUserId);
        Task<object> GetJobStatusAsync(Guid jobId);
    }

}
