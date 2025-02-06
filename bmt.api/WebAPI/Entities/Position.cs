using System;
using System.Collections.Generic;

namespace Core.API.Entities;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionCode { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public string? Description { get; set; }

    public bool IsActived { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
