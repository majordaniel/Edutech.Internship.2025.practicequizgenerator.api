namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IStudentCourseService
    {
        Task<List<string>> GetStudentCourseIdsAsync(string studentId);
    }
}
