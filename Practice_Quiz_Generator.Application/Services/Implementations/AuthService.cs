using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using Practice_Quiz_Generator.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Web;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        // Reminder --> Add logger 

        public AuthService(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public async Task<StandardResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            try
            {
                // Find user by email 
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return StandardResponse<LoginResponseDto>.Failed(
                        "Invalid credentials",
                        (int)HttpStatusCode.Unauthorized);
                }

                // Verify password using UserManager
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isPasswordValid)
                {
                    return StandardResponse<LoginResponseDto>.Failed(
                        "Invalid credentials",
                        (int)HttpStatusCode.Unauthorized);
                }

                // Check if user is active 
                if (!user.IsActive)
                {
                    return StandardResponse<LoginResponseDto>.Failed(
                        "Account is inactive",
                        (int)HttpStatusCode.Forbidden);
                }

                // Generate JWT token
                var token = await _jwtService.GenerateTokenAsync(user);
                var expiresAt = DateTime.UtcNow.AddMinutes(60); 

                var response = new LoginResponseDto
                {
                    Token = token,
                    ExpiresAt = expiresAt,
                    UserId = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Student"
                };

                return StandardResponse<LoginResponseDto>.Success("Login Successful", response);
            }
            catch (Exception ex)
            {
                return StandardResponse<LoginResponseDto>.Failed(
                    "An error occurred during login",
                    (int)HttpStatusCode.InternalServerError);
            }
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
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = await _emailService.GenerateEmailConfirmationLinkAsync(newUser.Email, emailConfirmationToken, "https");

                    var emailBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <style>
    body {{
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
      background-color: #f4f6f9;
      margin: 0;
      padding: 0;
      color: #333333;
    }}
    .container {{
      max-width: 600px;
      margin: 30px auto;
      background: #ffffff;
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 4px 12px rgba(0,0,0,0.08);
    }}
    .header {{
      background: #2C6BED; /* Academic Blue */
      color: white;
      text-align: center;
      padding: 25px;
    }}
    .header h2 {{
      margin: 0;
      font-size: 24px;
      font-weight: 600;
    }}
    .content {{
      padding: 30px;
      line-height: 1.7;
      font-size: 15px;
    }}
    .content p {{
      margin: 12px 0;
    }}
    .btn {{
      display: inline-block;
      padding: 12px 30px;
      margin: 25px 0;
      background-color: #2C6BED; /* Academic Blue */
      color: #ffffff !important;
      text-decoration: none;
      border-radius: 5px;
      font-weight: 600;
      font-size: 15px;
      transition: background 0.3s ease;
    }}
    .btn:hover {{
      background-color: #1f54b6;
    }}
    .footer {{
      background: #f9f9f9;
      text-align: center;
      padding: 18px;
      font-size: 12px;
      color: #777777;
      border-top: 1px solid #eeeeee;
    }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <h2>Confirm Your Email</h2>
    </div>
    <div class='content'>
      <p>Hi {newUser.FirstName},</p>
      <p>Welcome to <strong>Practice Quiz Generator</strong> 🎓.</p>
      <p>Your account has been created successfully, and you are now officially part of our academic community. 
      To activate your account and get started, please confirm your email address.</p>
      
      <p style='text-align:center;'>
        <a href='{confirmationLink}' class='btn'>Confirm My Email</a>
      </p>
      
      <p>If the button above does not work, please copy and paste the following link into your browser:</p>
      <p style='word-break:break-all;'><a href='{confirmationLink}'>{confirmationLink}</a></p>
      
      <p>Thank you for choosing PQG as your academic companion. We look forward to supporting your learning journey.</p>
      
      <p>Best regards,<br/><strong>The PQG Team</strong></p>
    </div>
    <div class='footer'>
      &copy; {DateTime.UtcNow.Year} Practice Quiz Generator | Empowering Academic Excellence
    </div>
  </div>
</body>
</html>";



                    //await _emailService.SendEmailAsync(newUser.Email, "Confirm Your Email", $"Please confirm your email by clicking this link: {confirmationLink}");
                    await _emailService.SendEmailAsync(newUser.Email, "Confirm Your Email", emailBody);
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


        //public async Task<StandardResponse<int>> BulkRegisterUsersAsync(IFormFile file)
        //{
        //    try
        //    {
        //        if (file == null || file.Length == 0)
        //        {
        //            return StandardResponse<int>.Failed("Invalid file");
        //        }



        //        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


        //    }
        //    catch (Exception ex)
        //    {
        //        return StandardResponse<int>.Failed($"Bulk upload failed: {ex.Message}");
        //    }
        //}


        public async Task<StandardResponse<ConfirmEmailResponseDto>> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return StandardResponse<ConfirmEmailResponseDto>.Failed("Invalid email.");


            //var decodedToken = WebUtility.UrlDecode(token);
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

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
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            //var resetLink = await _emailService.GeneratePasswordResetLinkAsync(user.Email, resetToken, "https");

            //await _emailService.SendEmailAsync(user.Email, "Reset Your Password", $"Reset your password: <a href='{resetLink}'>Click here</a>");

            //return StandardResponse<string>.Success("Password reset link has been sent to your email.", resetLink);

            // Update the user
            await _userManager.UpdateAsync(user);

            // Send email
            var resetLink = await _emailService.GeneratePasswordResetLinkAsync(user.Email, resetToken, "https");
            await _emailService.SendEmailAsync(user.Email, "Reset Your Password", $"Reset your password: <a href='{resetLink}'>Click here</a>");

            return StandardResponse<string>.Success(
                "Password reset link has been sent to your email.",
                resetLink);
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

