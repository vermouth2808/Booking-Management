using System;
using System.Collections.Generic;

namespace Core.Repositories.DataModel;

public partial class Staff
{
    public int StaffId { get; set; }

    public string StaffCode { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int PositionId { get; set; }

    public int StaffStatusId { get; set; }

    public string? Description { get; set; }

    public bool IsActived { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
