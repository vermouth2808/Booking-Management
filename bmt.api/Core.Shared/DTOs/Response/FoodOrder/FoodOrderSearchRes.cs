using Core.Shared.DTOs.Response.Movie;

namespace Core.Shared.DTOs.Response.FoodOrder
{
    public class FoodOrderSearchRes
    {
        public int TotalRecords { get; set; }
        public IEnumerable<FoodOrderRes> FoodOrders { get; set; }
    }
}
