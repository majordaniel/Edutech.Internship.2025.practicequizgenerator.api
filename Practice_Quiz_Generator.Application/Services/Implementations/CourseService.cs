using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StandardResponse<IEnumerable<CourseDto>>> GetAllCoursesAsync()
        {
            try
            {
                var courses = await _unitOfWork.CourseRepository.FindAll(false).ToListAsync();

                if (courses == null || !courses.Any())
                {

                    return StandardResponse<IEnumerable<CourseDto>>.Failed("No course found");
                }

                var coursesReturned = _mapper.Map<IEnumerable<CourseDto>>(courses);
                return StandardResponse<IEnumerable<CourseDto>>.Success("Courses successfully retrieved", coursesReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<IEnumerable<CourseDto>>.Failed(ex.Message);
            }
        }

    }
}

