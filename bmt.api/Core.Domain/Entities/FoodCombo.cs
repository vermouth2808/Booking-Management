namespace Core.Domain.Entities;

public partial class FoodCombo
{
    public int ComboId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public float Price { get; set; }

    public string? ImageUrl { get; set; }
    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<FoodOrder> FoodOrders { get; set; } = new List<FoodOrder>();
}
