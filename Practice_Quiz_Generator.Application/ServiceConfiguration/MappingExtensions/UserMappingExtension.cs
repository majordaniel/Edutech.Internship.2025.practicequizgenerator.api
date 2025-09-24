using Microsoft.AspNetCore.Identity;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions
{
    public static class UserMappingExtension
    {
        public static User ToEntity(this CreateUserRequestDto userRequest)
        {
            return new User()
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                OtherName = userRequest.OtherName,
                RegistrationNumber =userRequest.RegistrationNumber,
                Email = userRequest.Email
            };
        }

        public static UserResponseDto ToUserResponseDto(this IdentityResult entity, CreateUserRequestDto userRequest)
        {
            return new UserResponseDto
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                OtherName = userRequest.OtherName,
                RegistrationNumber = userRequest.RegistrationNumber,
                Email = userRequest.Email
            };
        }
    }
}
