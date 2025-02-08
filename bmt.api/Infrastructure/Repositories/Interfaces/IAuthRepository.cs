using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Auth;

namespace Core.Infrastructure.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<bool>> RegisterClient(UserRegisterReq req);
    }
}
