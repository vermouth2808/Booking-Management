using Core.Shared.Common.Models;
using Core.Shared.DTOs.Auth.Request;

namespace Core.Infrastructure.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<bool>> RegisterClient(UserRegisterReq req);
    }
}
