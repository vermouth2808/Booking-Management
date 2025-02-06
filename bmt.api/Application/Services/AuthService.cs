using Core.Application.Interfaces;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Auth.Request;

namespace Core.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserValidator _userValidator;
        public AuthService(IAuthRepository authRepository, IUserValidator userValidator)
        {
            _authRepository = authRepository;
            _userValidator = userValidator;
        }
        public async Task<Result<bool>> RegisterClient(UserRegisterReq req)
        {
            var validationResult = await _userValidator.ValidateUser(req);
            if (!validationResult.IsSuccess)
            {
                return Result<bool>.Failure(validationResult.Message);
            }
            var result = await _authRepository.RegisterClient(req);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Registration failed due to internal error.");
            }
            return Result<bool>.Success(true);
        }


    }
}
