using Core.Domain.Entities;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Auth.Request;
using Core.Shared.DTOs.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
