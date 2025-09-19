using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizSetupService : IQuizSetupService
    {
        private readonly IStudentCourseRepository _studentCourseRepository;

        public QuizSetupService(IStudentCourseRepository studentCourseRepository)
        {
            _studentCourseRepository = studentCourseRepository;
        }

        public async Task<QuizSetupResponseDto> GetQuizSetupDataAsync(string studentId)
        {
            var courses = await _studentCourseRepository.GetCoursesForStudentAsync(studentId);

            var courseDtos = courses.Select(c => new CourseDto { Id = int.Parse(c.Id), Name = c.Title }).ToList();

            return new QuizSetupResponseDto
            {
                Courses = courseDtos,
                QuestionTypes = new List<string> { "MultipleChoice", "Theory" },
                Sources = new List<string> { "PastExams", "UploadedMaterials" },
                MinQuestions = 5,
                MaxQuestions = 50
            };
        }
    }
}