using Core.Shared.Common.Models;

namespace Core.Shared.DTOs.Request.FoodOrder
{
    public class SearchFoodOrderReq : Search
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
