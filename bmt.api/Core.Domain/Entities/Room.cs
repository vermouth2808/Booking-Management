using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public string? Layout { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
    public ICollection<Showtime> Showtimes { get; set; }
}
