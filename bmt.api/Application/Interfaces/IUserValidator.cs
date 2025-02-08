using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Auth;

namespace Core.Application.Interfaces
{
    public interface IUserValidator
    {
        Task<Result<bool>> ValidateUser(UserRegisterReq req);
    }
}
