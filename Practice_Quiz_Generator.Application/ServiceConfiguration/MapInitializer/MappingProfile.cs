using AutoMapper;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.ServiceConfiguration.MapInitializer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FacultyRequestDto, Faculty>();
            CreateMap<Faculty, FacultyResponseDto>();
            CreateMap<CreateDepartmentRequestDto, Department>();
            CreateMap<Department, DepartmentResponseDto>();
            //CreateMap<>();
        }
    }
}
