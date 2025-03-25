using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class FoodOrder
{
    public int FoodOrderId { get; set; }

    public int? BookingId { get; set; }

    public int? ComboId { get; set; }

    public int Quantity { get; set; }

    public float TotalPrice { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual FoodCombo? Combo { get; set; }
}
