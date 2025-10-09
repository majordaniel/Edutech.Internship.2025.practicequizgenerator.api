using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

        public async Task<StandardResponse<UserResponseDto>> GetUserByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return StandardResponse<UserResponseDto>.Failed("Id field cannot be empty");
                }

                var student = await _unitOfWork.UserRepository.FindUserById(id);

                if (student == null)
                {
                    return StandardResponse<UserResponseDto>.Failed($"Student with Id {id} not found");
                }

                var studentReturned = _mapper.Map<UserResponseDto>(student);
                return StandardResponse<UserResponseDto>.Success("Student successfully retrieved", studentReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<UserResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<UserResponseDto>> GetUserByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return StandardResponse<UserResponseDto>.Failed("Email field cannot be empty");
                }

                var student = await _unitOfWork.UserRepository.FindUserByEmail(email);

                if (student == null)
                {
                    return StandardResponse<UserResponseDto>.Failed($"Student with email {email} not found");
                }

                var studentReturned = _mapper.Map<UserResponseDto>(student);
                return StandardResponse<UserResponseDto>.Success("Student successfully retrieved", studentReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<UserResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.FindAll(false).ToListAsync();

                if (users == null || !users.Any())
                {

                    return StandardResponse<IEnumerable<UserResponseDto>>.Failed("No student exist");
                }

                var usersReturned = _mapper.Map<IEnumerable<UserResponseDto>>(users);
                return StandardResponse<IEnumerable<UserResponseDto>>.Success("Users successfully retrieved", usersReturned);
            }
            catch (Exception ex)
            {
                return StandardResponse<IEnumerable<UserResponseDto>>.Failed(ex.Message);
            }
        }

    }
}
