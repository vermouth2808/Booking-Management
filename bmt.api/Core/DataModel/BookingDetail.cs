﻿using System;
using System.Collections.Generic;

namespace Core.Repositories.DataModel;

public partial class BookingDetail
{
    public int BookingDetailId { get; set; }

    public int BookingId { get; set; }

    public int SeatId { get; set; }

    /// <summary>
    /// giá vé từng ghế
    /// </summary>
    public double? Price { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
