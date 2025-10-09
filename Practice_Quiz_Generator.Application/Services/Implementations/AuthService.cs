using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.Constants;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;
        // Reminder --> Add logger 

        public AuthService(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
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

                //var createdUser = await _userManager.CreateAsync(newUser, userRequest.Password);
                var createdUser = await _userManager.CreateAsync(newUser, "PracticeQuiz@2025");
                await _userManager.AddToRoleAsync(newUser, "Student");



                if (createdUser.Succeeded)
                {
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = await _emailService.GenerateEmailConfirmationLinkAsync(newUser.Email, emailConfirmationToken, "https");

                    var emailBody = EmailTemplate.BuildWelcomeEmailTemplate(newUser.FirstName, confirmationLink);

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

        public async Task<StandardResponse<TokenDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return StandardResponse<TokenDto>.Failed("Invalid login credentials");
            }

            var tokens = await GenerateTokensAsync(user);
            return StandardResponse<TokenDto>.Success("Login successful", tokens);

        }

        public async Task<StandardResponse<TokenDto>> RefreshTokenAsync(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            if (principal == null)
                return StandardResponse<TokenDto>.Failed("Invalid access token");

            var email = principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return StandardResponse<TokenDto>.Failed("User not found");

            var storedRefreshToken = await _unitOfWork.RefreshTokenRepository
                .GetValidRefreshTokenAsync(tokenDto.RefreshToken, user.Id);
                //.FindByCondition(r => r.Token == tokenDto.RefreshToken && r.UserId == user.Id, false);

            if (storedRefreshToken == null || storedRefreshToken.ExpiryDate <= DateTime.UtcNow || storedRefreshToken.IsUsed)
                return StandardResponse<TokenDto>.Failed("Invalid refresh token");

            // mark old refresh token as used
            storedRefreshToken.IsUsed = true;
            _unitOfWork.RefreshTokenRepository.Update(storedRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            var newTokens = await GenerateTokensAsync(user);
            return StandardResponse<TokenDto>.Success("Token refreshed", newTokens);

        }


        private async Task<TokenDto> GenerateTokensAsync(User user)
        {
            var claims = await GetClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:securityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:validIssuer"],
                audience: _configuration["JwtSettings:validAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenExpiryMinutes"])),
                signingCredentials: creds
            );


            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenExpiryDays"]))
            };

            await _unitOfWork.RefreshTokenRepository.CreateAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:securityKey"])),
                    ValidateLifetime = false,
                    ValidIssuer = _configuration["JwtSettings:validIssuer"],
                    ValidAudience = _configuration["JwtSettings:validAudience"]
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }

            //    var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var securityToken);
            //    if (securityToken is not JwtSecurityToken jwtToken ||
            //        !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            //        return null;

            //    return principal;
            //}
            //catch
            //{
            //    return null;
            //}
        }
    }
}

