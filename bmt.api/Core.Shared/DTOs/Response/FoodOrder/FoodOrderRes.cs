namespace Core.Shared.DTOs.Response.FoodOrder
{
    public class FoodOrderRes
    {
        public int FoodOrderId { get; set; }

        public int? BookingId { get; set; }

        public int? ComboId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
