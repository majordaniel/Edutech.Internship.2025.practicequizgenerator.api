using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Net;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        // Reminder --> Add logger 

        public AuthService(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<StandardResponse<UserResponseDto>> RegisterAsync(CreateUserRequestDto userRequest)
        {
            try
            {
                if (userRequest == null)
                {
                    return StandardResponse<UserResponseDto>.Failed("Invalid request: user details are required.");
                }

                var userExists = await _unitOfWork.UserRepository
                    .FindUserByEmail(userRequest.Email);

                if (userExists != null)
                {
                    return StandardResponse<UserResponseDto>.Failed("User with this email already exists.");
                }

                //var userFaculty = await _unitOfWork.FacultyRepository
                //    .FindFacultyByName(userRequest.FacultyName);

                //if (userFaculty == null)
                //{
                //    return StandardResponse<UserResponseDto>.Failed(
                //        $"Faculty '{userRequest.FacultyName}' not found."
                //    );
                //}


                //var userDepartment = await _unitOfWork.DepartmentRepository
                //    .FindDepartmentByName(userRequest.DepartmentName);

                //if (userDepartment == null)
                //{
                //    return StandardResponse<UserResponseDto>.Failed(
                //        $"Department '{userRequest.DepartmentName}' not found."
                //    );
                //}

                //var userLevel = await _unitOfWork.LevelRepository
                //    .FindLevelByCode(userRequest.CurrentLevelCode);

                //if (userLevel == null)
                //{
                //    return StandardResponse<UserResponseDto>.Failed(
                //        $"Level '{userRequest.CurrentLevelCode}' not found."
                //    );
                //}

                var userFaculty = await _unitOfWork.FacultyRepository.FindFacultyByName(userRequest.FacultyName);
                var userDepartment = await _unitOfWork.DepartmentRepository.FindDepartmentByName(userRequest.DepartmentName);
                var userLevel = await _unitOfWork.LevelRepository.FindLevelByCode(userRequest.CurrentLevelCode);

                if (userFaculty == null || userDepartment == null || userLevel == null)
                    return StandardResponse<UserResponseDto>.Failed("Invalid Faculty, Department, or Level.");

                var newUser = userRequest.ToEntity();
                newUser.FacultyId = userFaculty.Id;
                newUser.DepartmentId = userDepartment.Id;
                newUser.CurrentLevelId = userLevel.Id;
                newUser.UserName = userRequest.Email;
                //User newUser = _mapper.Map<User>(userRequest);


                var createdUser = await _userManager.CreateAsync(newUser, userRequest.Password);
                await _userManager.AddToRoleAsync(newUser, "Student");

                if (createdUser.Succeeded)
                {
                    //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    //var confirmationLink = await _emailService.GenerateEmailConfirmationLinkAsync(newUser.Email, emailConfirmationToken, "https");

                    //await _emailService.CreateEmail(newUser.Email, "Confirm Your Email", $"Please confirm your email by clicking this link: {confirmationLink}");

                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = await _emailService.GenerateEmailConfirmationLinkAsync(newUser.Email, emailConfirmationToken, "https");

                    await _emailService.SendEmailAsync(newUser.Email, "Confirm Your Email", $"Please confirm your email by clicking this link: {confirmationLink}");

                  

                }
                else if (!createdUser.Succeeded)
                {
                    return StandardResponse<UserResponseDto>.Failed($"User registration failed.");
                }

                var userReturned = createdUser.ToUserResponseDto(userRequest);
                userReturned.FacultyId = userFaculty.Id;
                userReturned.DepartmentId = userDepartment.Id;
                userReturned.CurrentLevelId = userLevel.Id;

                return StandardResponse<UserResponseDto>.Success("User registered successfully. Please check your email for confirmation.", userReturned);
            
            }
            catch (Exception ex)
            {
                return StandardResponse<UserResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<ConfirmEmailResponseDto>> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return StandardResponse<ConfirmEmailResponseDto>.Failed("Invalid email.");


            token = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return StandardResponse<ConfirmEmailResponseDto>.Failed("Email confirmation failed.");

            return StandardResponse<ConfirmEmailResponseDto>.Success("Email confirmed successfully.", new ConfirmEmailResponseDto { Email = user.Email, IsConfirmed = true });
        }

        public async Task<StandardResponse<string>> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StandardResponse<string>.Failed("No account found with this email.");
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return StandardResponse<string>.Failed("Email not confirmed. Please confirm your email before resetting password.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = await _emailService.GeneratePasswordResetLinkAsync(user.Email, resetToken, "https");

            await _emailService.SendEmailAsync(user.Email, "Reset Your Password", $"Reset your password: <a href='{resetLink}'>Click here</a>");

            return StandardResponse<string>.Success("Password reset link has been sent to your email.", resetLink);
        }

        public async Task<StandardResponse<string>> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return StandardResponse<string>.Failed("Invalid email.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
                return StandardResponse<string>.Failed("Password reset failed. Token may be invalid or expired.");

            return StandardResponse<string>.Success("Password has been reset successfully.", user.Email);
        }


      

    }
}

