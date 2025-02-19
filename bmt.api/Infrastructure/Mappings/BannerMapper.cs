using Core.Domain.Entities;
using Core.Shared.DTOs.Response.Banner;

namespace Core.Infrastructure.Mappings
{
    public interface IBannerMapper<T> where T : BannerRes, new()
    {
        T ToModel(Banner dataModel);
        Banner ToDataModel(T model);
        BannerRes ToModel<T>(T data) where T : BannerRes, new();
    }

    public class BannerMapper<T> : IBannerMapper<T> where T : BannerRes, new()
    {
        public BannerMapper() { }

        // Chuyển từ BannerRes (DTO) sang Banner (Entity)
        public Banner ToDataModel(T model)
        {
            var result = new Banner
            {
                BannerId = model.BannerId,
                BannerName = model.BannerName,
                BannerUrl = model.BannerUrl,
                CreatedDate = DateTime.Now,
                CreatedUserId = 1,
                IsDeleted = false,
            };

            return result;
        }

        // Chuyển từ Banner (Entity) sang BannerRes (DTO)
        public T ToModel(Banner dataModel)
        {
            var result = new T
            {
                BannerId = dataModel.BannerId,
                BannerName = dataModel.BannerName,
                BannerUrl = dataModel.BannerUrl,
            };

            return result;
        }

        public BannerRes ToModel<T>(T data) where T : BannerRes, new()
        {
            return new BannerRes
            {
                BannerId = data.BannerId,
                BannerName = data.BannerName,
                BannerUrl = data.BannerUrl,
            };
        }
    }
}
