using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

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

        public async Task<StandardResponse<DepartmentResponseDto>> CreateDepartmentAsync(CreateDepartmentRequestDto departmentRequest)
        {
            try
            {
                if (departmentRequest == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed("Invalid department request. Creation failed.");
                }

                var existingDepartment = await _unitOfWork.DepartmentRepository
                    .FindDepartmentByName(departmentRequest.Name);

                if (existingDepartment != null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed(
                        $"A department with the name '{departmentRequest.Name}' already exists."
                    );
                }

                var departmentFaculty = await _unitOfWork.FacultyRepository
                    .FindFacultyByName(departmentRequest.FacultyName);

                if (departmentFaculty == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed(
                        $"Faculty '{departmentRequest.FacultyName}' not found."
                    );
                }

                var newDepartment = departmentRequest.ToEntity();
                newDepartment.FacultyId = departmentFaculty.Id;

                await _unitOfWork.DepartmentRepository.CreateAsync(newDepartment);
                await _unitOfWork.SaveChangesAsync();


                var departmentReturned = newDepartment.ToResponseDto();
                //var departmentReturned = _mapper.Map<DepartmentResponseDto>(newDepartment);
                return StandardResponse<DepartmentResponseDto>.Success($"Departmet successfully created: {newDepartment.Name}", departmentReturned);
            }

            catch (Exception ex)
            {
                return StandardResponse<DepartmentResponseDto>.Failed(ex.Message);
            }
        }

        // Reminder --> Bulk Upload
        // GetDepartmetByFaculty
        public async Task<StandardResponse<IEnumerable<DepartmentResponseDto>>> GetAllDepartmentAsync()
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.FindAll(false).ToListAsync();


                if (departments == null || !departments.Any())
                {

                    return StandardResponse<IEnumerable<DepartmentResponseDto>>.Failed("No department exist");
                }

                var departmentsReturned = departments.ToResponseDtoList();
                //var departmentsReturned = _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
                return StandardResponse<IEnumerable<DepartmentResponseDto>>.Success("Departments successfully retrieved", departmentsReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<IEnumerable<DepartmentResponseDto>>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<IEnumerable<DepartmentResponseDto>>> GetAllActiveDepartmentAsync()
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository
                    .FindAll(false)
                    .Where(d => d.IsDeleted == false)
                    .ToListAsync();


                if (departments == null || !departments.Any())
                {

                    return StandardResponse<IEnumerable<DepartmentResponseDto>>.Failed("No department exist");
                }

                var departmentsReturned = _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
                return StandardResponse<IEnumerable<DepartmentResponseDto>>.Success("Department successfully retrieved", departmentsReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<IEnumerable<DepartmentResponseDto>>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<DepartmentResponseDto>.Failed("Id cannot be empty");
                }

                var department = await _unitOfWork.DepartmentRepository.FindDepartmentById(id);

                if (department == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed($"Department with Id {id} not found");
                }

                var departmentReturned = _mapper.Map<DepartmentResponseDto>(department);
                return StandardResponse<DepartmentResponseDto>.Success("Department successfully retrieved", departmentReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<DepartmentResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<DepartmentResponseDto>> GetDepartmentByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return StandardResponse<DepartmentResponseDto>.Failed("Department name cannot be empty");
                }

                var department = await _unitOfWork.DepartmentRepository.FindDepartmentByName(name);

                if (department == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed($"Department with name {name} not found");
                }

                var departmentReturned = _mapper.Map<DepartmentResponseDto>(department);
                return StandardResponse<DepartmentResponseDto>.Success("Department successfully retrieved", departmentReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<DepartmentResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<DepartmentResponseDto>> UpdateDepartmentAsync(string id, CreateDepartmentRequestDto departmentRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<DepartmentResponseDto>.Failed("Id cannot be empty");
                }

                if (departmentRequest == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed("Department update failed");
                }

                var department = await _unitOfWork.DepartmentRepository.FindDepartmentById(id);
                if (department == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed($"Department with Id {id} not found");
                }

                //var updatedDepartment = _mapper.Map(departmentRequest, department);

                var departmentFaculty = await _unitOfWork.FacultyRepository
                    .FindFacultyByName(departmentRequest.FacultyName);

                if (departmentFaculty == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed(
                        $"Faculty '{departmentRequest.FacultyName}' not found."
                    );
                }
                department.ToEntityUpdate(departmentRequest);
                department.FacultyId = departmentFaculty.Id;

                //department.Name = departmentRequest.Name;
                //department.Code = departmentRequest.Code;
                //department.Description = departmentRequest.Description;
                //department.HOD = departmentRequest.HOD;
                //department.FacultyId = departmentFaculty.Id;
                //department.DateModified = DateTime.UtcNow;

                _unitOfWork.DepartmentRepository.Update(department);
                await _unitOfWork.SaveChangesAsync();

                var departmentReturned = _mapper.Map<DepartmentResponseDto>(department);
                return StandardResponse<DepartmentResponseDto>.Success("Department successfully updated", departmentReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<DepartmentResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<DepartmentResponseDto>> DeleteDepartmnetAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<DepartmentResponseDto>.Failed("Id cannot be empty");
                }

                var department = await _unitOfWork.DepartmentRepository.FindDepartmentById(id);

                if (department == null)
                {
                    return StandardResponse<DepartmentResponseDto>.Failed($"Department with Id {id} does not exist");
                }

                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.SaveChangesAsync();

                var departmentDeleted = _mapper.Map<DepartmentResponseDto>(department);

                return StandardResponse<DepartmentResponseDto>.Success("Department successfully deleted", departmentDeleted);
            }
            catch (Exception ex)
            {
                return StandardResponse<DepartmentResponseDto>.Failed(ex.Message);
            }
        }
    }
}
