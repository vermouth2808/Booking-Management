using Core.Shared.Common.Models;
using Core.Shared.DTOs.Auth.Request;

namespace Core.Application.Interfaces
{
    public interface IUserValidator
    {
        Task<Result<bool>> ValidateUser(UserRegisterReq req);
    }
}
