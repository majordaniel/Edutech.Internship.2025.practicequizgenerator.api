using System.Threading.Tasks;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExamPortalContext _context;
        private ICourseRepository _courseRepository;
        private IDepartmentRepository _departmentRepository;
        private IFacultyRepository _faultyRepository;
        private ILevelRepository _levelRepository;
        private IStudentCourseRepository _studentCourseRepository;
        private IUserRepository _userRepository;
        private IQuizRepository _quizRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IQuizAttemptRepository _quizAttemptRepository;
        private IUserResponseRepository _userResponseRepository;
        private IQuestionBankRepository _questionBankRepository;

        public UnitOfWork(ExamPortalContext context)
        {
            _context = context;
        }


        public ICourseRepository CourseRepository
        {
            get
            {
                if (_courseRepository == null)
                    _courseRepository = new CourseRepository(_context);
                return _courseRepository;
            }
        }

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                if (_departmentRepository == null)
                    _departmentRepository = new DepartmentRepository(_context);
                return _departmentRepository;
            }
        }

        public IFacultyRepository FacultyRepository
        {
            get
            {
                if (_faultyRepository == null)
                    _faultyRepository = new FacultyRepository(_context);
                return _faultyRepository;
            }
        }

        public ILevelRepository LevelRepository
        {
            get
            {
                if (_levelRepository == null)
                    _levelRepository = new LevelRepository(_context);
                return _levelRepository;
            }
        }
        public IStudentCourseRepository StudentCourseRepository
        {
            get
            {
                if (_studentCourseRepository == null)
                    _studentCourseRepository = new StudentCourseRepository(_context);
                return _studentCourseRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public IQuizRepository QuizRepository 
        { 
            get 
            { 
                if (_quizRepository == null)
                    _quizRepository = new QuizRepository(_context);
                return _quizRepository; 
            } 
        }

        public IRefreshTokenRepository RefreshTokenRepository 
        { 
            get 
            { 
                if (_refreshTokenRepository == null)
                    _refreshTokenRepository = new RefreshTokenRepository(_context);
                return _refreshTokenRepository; 
            } 
        }
        public IQuizAttemptRepository QuizAttemptRepository
        {
            get
            {
                if (_quizAttemptRepository == null)
                    _quizAttemptRepository = new QuizAttemptRepository(_context);
                return _quizAttemptRepository;
            }
        }

        public IUserResponseRepository UserResponseRepository
        {
            get
            {
                if (_userResponseRepository == null)
                    _userResponseRepository = new UserResponseRepository(_context);
                return _userResponseRepository;
            }
        }
        public IQuestionBankRepository QuestionBankRepository
        {
            get
            {
                if (_questionBankRepository == null)
                    _questionBankRepository = new QuestionBankRepository(_context);
                return _questionBankRepository;
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

