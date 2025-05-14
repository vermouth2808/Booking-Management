namespace Core.Shared.DTOs.Request.FoodCombo
{
    public class CreateFoodComboReq
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public float Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
