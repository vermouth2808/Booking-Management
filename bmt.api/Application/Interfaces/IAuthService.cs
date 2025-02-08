using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Auth;

namespace Core.Application.Interfaces
{
    public interface IAuthService  
    {
        /// <summary>
        /// register account client
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Result<bool>> RegisterClient(UserRegisterReq req);
    }
}
