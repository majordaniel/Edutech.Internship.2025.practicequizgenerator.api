using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryBase<User> _repositoryBase;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        // Reminder --> Add logger 

        public AuthService(UserManager<User> userManager, IMapper mapper, RepositoryBase<User> repositoryBase)
        {
            _repositoryBase = repositoryBase;
            _userManager = userManager;
            _mapper = mapper;
        }

        //public async Task<StandardResponse<IdentityResult>> Register(StudentRequestDto studentRequest)
        //{
        //    try
        //    {
        //        if (studentRequest == null)
        //        {
        //            return null;
        //        }

        //        var studentExit = _repositoryBase.FindByCondition(s => s.Email == studentRequest.Email, false);

        //        if(studentExit == null)
        //        {
        //            StandardResponse<IdentityResult>.Failed("User already exist");
        //        }




        //        throw new NotImplementedException();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }
}
