using Microsoft.AspNetCore.Http;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Data;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuestionBankService
    {
        Task<IEnumerable<QuestionBank>> MapDataTableToQuestionBankAsync(DataTable table, string courseTitle);
        Task ImportFromExcelAsync(IFormFile file, string courseTitle);
        Task<StandardResponse<IEnumerable<QuestionBankResponseDto>>> GetAllQuestionsAsync();
        Task<StandardResponse<QuestionByCourseResponseDto>> GetAllQuestionByCourseIdAsync(string courseId);
        Task<StandardResponse<QuestionByCourseResponseDto>> GetAllQuestionByCourseTitleAsync(string courseTitle);
    }
}
