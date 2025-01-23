using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public bool IsActived { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
}
