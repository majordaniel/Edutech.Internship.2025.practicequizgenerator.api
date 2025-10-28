using Microsoft.AspNetCore.Http;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Data;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExcelService _excelService;

        public QuestionBankService(IUnitOfWork unitOfWork, IExcelService excelSercice)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelSercice;
        }

        public async Task<StandardResponse<QuestionByCourseResponseDto>> GetAllQuestionByCourseIdAsync(string courseId)
        {
            try
            {
                var questions = await _unitOfWork.QuestionBankRepository.FindAllQuestionByCourseId(courseId);

                if (questions == null || !questions.Any())
                    return StandardResponse<QuestionByCourseResponseDto>.Failed("No questions found for this course.");


                var totalQuestions = questions.Count();

                var questionsToReturn = new QuestionByCourseResponseDto
                {
                    CourseTitle = questions.FirstOrDefault().Course.Title,
                    TotalQuestions = totalQuestions,
                    Questions = questions.Select(q => new QuestionBankResponseDto
                    {
                        Id = q.Id,
                        Text = q.Text,
                        QuestionType = q.QuestionType,
                        CorrectAnswer = q.CorrectAnswer,
                        Source = q.Source,
                        CourseId = q.CourseId,


                        Options = q.Option.Select(o => new QuestionBankOptionDto
                        {
                            OptionLabel = o.OptionLabel,
                            OptionText = o.OptionText,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    }).ToList()
                };
                return StandardResponse<QuestionByCourseResponseDto>.Success("Questions retrieved successfully.", questionsToReturn);

            }
            catch (Exception ex)
            {
                return StandardResponse<QuestionByCourseResponseDto>.Failed($"An error occurred while retrieving questions: {ex.Message}");
            }
        }

        public async Task<StandardResponse<QuestionByCourseResponseDto>> GetAllQuestionByCourseTitleAsync(string courseTitle)
        {
            try
            {
                var questions = await _unitOfWork.QuestionBankRepository.FindAllQuestionByCourseTitle(courseTitle);

                if (questions == null || !questions.Any())
                    return StandardResponse<QuestionByCourseResponseDto>.Failed("No questions found for this course.");

                var totalQuestions = questions.Count();

                var questionsToReturn = new QuestionByCourseResponseDto
                {
                    CourseTitle = questions.FirstOrDefault().Course.Title,
                    TotalQuestions = totalQuestions,
                    Questions = questions.Select(q => new QuestionBankResponseDto
                    {
                        Id = q.Id,
                        Text = q.Text,
                        QuestionType = q.QuestionType,
                        CorrectAnswer = q.CorrectAnswer,
                        Source = q.Source,
                        CourseId = q.CourseId,


                        Options = q.Option.Select(o => new QuestionBankOptionDto
                        {
                            OptionLabel = o.OptionLabel,
                            OptionText = o.OptionText,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    }).ToList()
                };
                return StandardResponse<QuestionByCourseResponseDto>.Success("Questions retrieved successfully.", questionsToReturn);

            }
            catch (Exception ex)
            {
                return StandardResponse<QuestionByCourseResponseDto>.Failed($"An error occurred while retrieving questions: {ex.Message}");
            }
        }

        public async Task<StandardResponse<IEnumerable<QuestionBankResponseDto>>> GetAllQuestionsAsync()
        {
            try
            {
                var questions = await _unitOfWork.QuestionBankRepository.FindAllQuestionWithOption();

                if (questions == null || !questions.Any())
                    return StandardResponse<IEnumerable<QuestionBankResponseDto>>.Failed("No questions found.");

                var response = questions.Select(q => new QuestionBankResponseDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    QuestionType = q.QuestionType,
                    CorrectAnswer = q.CorrectAnswer,
                    Source = q.Source,
                    CourseId = q.CourseId,
                    Options = q.Option.Select(o => new QuestionBankOptionDto
                    {
                        OptionLabel = o.OptionLabel,
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                });

                return StandardResponse<IEnumerable<QuestionBankResponseDto>>.Success("Questions retrieved successfully.", response);

            }
            catch (Exception ex)
            {
                return StandardResponse<IEnumerable<QuestionBankResponseDto>>.Failed($"An unexpected error occurred: {ex.Message}");

            }
        }

        public async Task ImportFromExcelAsync(IFormFile file, string courseTitle)
        {
            try
            {
                var table = await _excelService.ExtractFileDataAsync(file);
                var questions = await MapDataTableToQuestionBankAsync(table, courseTitle);

                await _unitOfWork.QuestionBankRepository.CreateManyAsync(questions);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred while importing questions for '{courseTitle}'.", ex);
            }
        }

        public async Task<IEnumerable<QuestionBank>> MapDataTableToQuestionBankAsync(DataTable table, string courseTitle)
        {
            try
            {
                var questions = new List<QuestionBank>();

                var course = await _unitOfWork.CourseRepository.FindCourseByName(courseTitle);

                if (course == null)
                {
                    throw new KeyNotFoundException($"Course with title '{courseTitle}' was not found.");
                }

                foreach (DataRow row in table.Rows)
                {
                    string questionText = row["Question"]?.ToString()?.Trim() ?? string.Empty;
                    string optionA = row["OptionA"]?.ToString()?.Trim() ?? string.Empty;
                    string optionB = row["OptionB"]?.ToString()?.Trim() ?? string.Empty;
                    string optionC = row["OptionC"]?.ToString()?.Trim() ?? string.Empty;
                    string optionD = row["OptionD"]?.ToString()?.Trim() ?? string.Empty;
                    string answer = row["Answer"]?.ToString()?.Trim().ToUpper() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(questionText))
                        continue;

                    var question = new QuestionBank
                    {
                        Text = questionText,
                        CorrectAnswer = answer,
                        Source = "Excel Upload",
                        CourseId = course.Id, //"e7a9b6d8-0c2c-4a68-a777-4cd5aa3b68ad", //Reminder --> Make this dynamic
                        Option = new List<QuestionBankOption>
                    {
                        new() { OptionLabel = "A", OptionText = optionA, IsCorrect = answer == "A" },
                        new() { OptionLabel = "B", OptionText = optionB, IsCorrect = answer == "B" },
                        new() { OptionLabel = "C", OptionText = optionC, IsCorrect = answer == "C" },
                        new() { OptionLabel = "D", OptionText = optionD, IsCorrect = answer == "D" }
                    }
                    };

                    questions.Add(question);
                }

                return questions;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while mapping question bank data: {ex.Message}", ex);
            }
        }

    }
}
