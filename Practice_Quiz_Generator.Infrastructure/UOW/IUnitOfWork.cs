using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IFacultyRepository FacultyRepository { get; }
        ILevelRepository LevelRepository { get; }
        IStudentCourseRepository StudentCourseRepository { get; }
        IUserRepository UserRepository { get; }
        IQuizRepository QuizRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IQuestionBankRepository QuestionBankRepository { get; }
        Task SaveChangesAsync();
    }
}
