using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.DTOs.Request.Banner
{
    public class CreateBannerReq
    {
        public string BannerName { get; set; } = null!;

        public string BannerUrl { get; set; } = null!;

    }
}
