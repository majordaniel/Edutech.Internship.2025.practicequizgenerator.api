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

        public UnitOfWork(ExamPortalContext context)
        {
            _context = context;
        }


        public ICourseRepository CourseRepository
        {
            get 
            { 
                if(_courseRepository == null) 
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

        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
