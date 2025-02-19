using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.DTOs.Request.Banner
{
    public class UpdateBannerReq
    {
        public int BannerId { get; set; }

        public string BannerName { get; set; } = null!;

        public string BannerUrl { get; set; } = null!;

    }
}
