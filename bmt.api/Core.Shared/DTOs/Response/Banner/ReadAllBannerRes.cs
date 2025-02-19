namespace Core.Shared.DTOs.Response.Banner
{
    public class ReadAllBannerRes
    {
        public int TotalRecords { get; set; }
        public IEnumerable<BannerRes> Banners { get; set; }
    }
}
