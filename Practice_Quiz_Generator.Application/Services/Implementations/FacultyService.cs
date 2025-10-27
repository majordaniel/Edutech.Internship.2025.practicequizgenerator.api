using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class FacultyService : IFacultyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FacultyService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

        public async Task<StandardResponse<FacultyResponseDto>> CreateFacultyAsync(FacultyRequestDto facultyRequest)
        {
            try
            {
                if (facultyRequest == null)
                {
                    return StandardResponse<FacultyResponseDto>.Failed("Invalid faculty request.Creation failed.");
                }

                var existingFaculty = await _unitOfWork.FacultyRepository
                    .FindFacultyByName(facultyRequest.Name);

                if (existingFaculty != null)
                {
                    return StandardResponse<FacultyResponseDto>.Failed(
                        $"A faculty with the name '{facultyRequest.Name}' already exists."
                    );
                }

                var newFaculty = _mapper.Map<Faculty>(facultyRequest);
                await _unitOfWork.FacultyRepository.CreateAsync(newFaculty);
                await _unitOfWork.SaveChangesAsync();

                var facultyToReturn = _mapper.Map<FacultyResponseDto>(newFaculty);
                return StandardResponse<FacultyResponseDto>.Success($"Faculty successfully created: {newFaculty.Name}", facultyToReturn);
            }


            catch (Exception ex)
            {
                return StandardResponse<FacultyResponseDto>.Failed(ex.Message);
            }
        }

        // Reminder --> Bulk Faculty Upload

        public async Task<StandardResponse<IEnumerable<FacultyResponseDto>>> GetAllFacultiesAsync()
        {
            try
            {
                var faculties = await _unitOfWork.FacultyRepository.FindAll(false).ToListAsync();

                if (faculties == null || !faculties.Any())
                {

                    return StandardResponse<IEnumerable<FacultyResponseDto>>.Failed("No faculties exist");
                }

                var facultiesReturned = _mapper.Map<IEnumerable<FacultyResponseDto>>(faculties);
                return StandardResponse<IEnumerable<FacultyResponseDto>>.Success("Faculties successfully retrieved", facultiesReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<IEnumerable<FacultyResponseDto>>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<FacultyResponseDto>> GetFacultyByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<FacultyResponseDto>.Failed("Id field cannot be empty");
                }

                var faculty = await _unitOfWork.FacultyRepository.FindFacultyById(id);

                if (faculty == null)
                {
                    return StandardResponse<FacultyResponseDto>.Failed($"Faculty with Id {id} not found");
                }

                var facultyReturned = _mapper.Map<FacultyResponseDto>(faculty);
                return StandardResponse<FacultyResponseDto>.Success("Faculty successfully retrieved", facultyReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<FacultyResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<FacultyResponseDto>> GetFacultyByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return StandardResponse<FacultyResponseDto>.Failed("Faculty name cannot be empty");
                }

                var faculty = await _unitOfWork.FacultyRepository.FindFacultyByName(name);

                if (faculty == null)
                {
                    return StandardResponse<FacultyResponseDto>.Failed($"Faculty with name {name} not found");
                }

                var facultyReturned = _mapper.Map<FacultyResponseDto>(faculty);
                return StandardResponse<FacultyResponseDto>.Success("Faculty successfully retrieved", facultyReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<FacultyResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<FacultyResponseDto>> UpdateFacultyAsync(string id, FacultyRequestDto facultyRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<FacultyResponseDto>.Failed("Id cannot be empty");
                }

                if (facultyRequest == null)
                {
                    return StandardResponse<FacultyResponseDto>.Failed("Faculty update failed");
                }

                var faculty = await _unitOfWork.FacultyRepository.FindFacultyById(id);
                if (faculty == null)
                {
                    return StandardResponse<FacultyResponseDto>.Failed($"Faculty with Id {id} not found");
                }

                var updatedFaculty = _mapper.Map(facultyRequest, faculty);
                _unitOfWork.FacultyRepository.Update(updatedFaculty);
                await _unitOfWork.SaveChangesAsync();

                var facultyToReturn = _mapper.Map<FacultyResponseDto>(updatedFaculty);
                return StandardResponse<FacultyResponseDto>.Success("Faculty successfully updated", facultyToReturn);
            }
            catch (Exception ex)
            {
                return StandardResponse<FacultyResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<Faculty>> DeleteFacultyAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<Faculty>.Failed("Id cannot be empty");
                }

                var faculty = await _unitOfWork.FacultyRepository.FindFacultyById(id);
                if (faculty == null)
                {
                    return StandardResponse<Faculty>.Failed($"Faculty with Id {id} does not exist");
                }

                _unitOfWork.FacultyRepository.Delete(faculty);
                await _unitOfWork.SaveChangesAsync();

                return StandardResponse<Faculty>.Success("Faculty successfully deleted", faculty);
            }
            catch (Exception ex)
            {
                return StandardResponse<Faculty>.Failed(ex.Message);
            }
        }
    }
}
