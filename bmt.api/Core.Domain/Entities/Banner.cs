using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class Banner
{
    public int BannerId { get; set; }

    public string BannerName { get; set; } = null!;

    public string BannerUrl { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
