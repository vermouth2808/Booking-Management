using System;
using System.Collections.Generic;

namespace Core.Repositories.DataModel;

public partial class Seat
{
    public int SeatId { get; set; }

    public string? Room { get; set; }

    /// <summary>
    /// Số ghế (ví dụ: A1, A2)
    /// </summary>
    public string? SeatNumber { get; set; }

    /// <summary>
    /// 1:ghế VIP, 0 : ghế thường
    /// </summary>
    public bool? IsVip { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
