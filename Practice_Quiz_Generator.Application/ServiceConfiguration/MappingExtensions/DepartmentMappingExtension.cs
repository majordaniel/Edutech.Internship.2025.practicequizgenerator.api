using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions
{
    public static class DepartmentMappingExtension
    {

        public static Department ToEntity(this CreateDepartmentRequestDto departmentRequest)
        {
            return new Department()
            {
                Name = departmentRequest.Name,
                Code = departmentRequest.Code,
                Description = departmentRequest.Description,
                HOD = departmentRequest.HOD
            };
        }

        public static DepartmentResponseDto  ToResponseDto(this Department department)
        {
            return new DepartmentResponseDto()
            {
                Name = department.Name,
                Code = department.Code,
                Description = department.Description,
                HOD = department.HOD,
                FacultyId = department.FacultyId,
                DateCreated = department.DateCreated,
                DateModified = department.DateModified,
                CreatedBy = department.CreatedBy,
                ModifiedBy = department.ModifiedBy,
                Status = department.Status,
                IsDeleted = department.IsDeleted,

            };
        }

        public static Department ToEntityUpdate(this Department department, CreateDepartmentRequestDto updateRequest)
        {
            department.Name = updateRequest.Name;
            department.Code = updateRequest.Code;
            department.Description = updateRequest.Description;
            department.HOD = updateRequest.HOD;
            department.DateModified = DateTime.UtcNow;

            return department;
        }

        public static IEnumerable<DepartmentResponseDto> ToResponseDtoList(this IEnumerable<Department> departments)
        {
            return departments.Select(d => d.ToResponseDto()).ToList();
        }
    }
}
