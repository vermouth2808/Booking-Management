namespace Core.Shared.DTOs.Request.FoodOrder
{
    public class CreateFoodOrderReq
    {
        public int? BookingId { get; set; }

        public int? ComboId { get; set; }

        public int Quantity { get; set; }

        public float TotalPrice { get; set; }
    }
}
