using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Banner;
using Core.Shared.DTOs.Response.Banner;

namespace Core.Infrastructure.Repositories.Interfaces
{
    public interface IBannerRepository<T> where T : BannerRes, new()
    {
        Task<Result<T>> GetBannerById(int id);
        Task<Result<ReadAllBannerRes>> ReadAllBanner();
        Task<Result<bool>> CreateBanner(CreateBannerReq req, int CreatedUserId);
        Task<Result<bool>> UpdateBanner(UpdateBannerReq req, int CreatedUserId);
        Task<Result<bool>> DeleteBanner(int id, int CreatedUserId);
    }
}
