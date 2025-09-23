using AutoMapper;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

    }
}
