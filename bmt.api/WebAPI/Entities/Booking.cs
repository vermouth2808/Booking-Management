using System;
using System.Collections.Generic;

namespace Core.API.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int? ShowtimeId { get; set; }

    /// <summary>
    /// thời gian đặt vé
    /// </summary>
    public DateTime? BookingTime { get; set; }

    public double? TotalAmount { get; set; }

    /// <summary>
    /// Pending, Paid, Cancelled
    /// </summary>
    public string? PaymentStatus { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
