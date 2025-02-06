using System;
using System.Collections.Generic;

namespace Core.API.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? FullName { get; set; }

    public string PassWord { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsOnline { get; set; }

    public bool IsActive { get; set; }

    public bool IsSuperAdmin { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
