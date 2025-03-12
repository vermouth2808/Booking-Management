using Core.Shared.Common.Models;

namespace Core.Shared.DTOs.Request.ShowTime
{
    public class SearchShowTimeReq : Search
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
