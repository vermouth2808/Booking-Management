using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int ShowtimeId { get; set; }

    public int SeatId { get; set; }

    public int CustomerId { get; set; }

    public double Price { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
