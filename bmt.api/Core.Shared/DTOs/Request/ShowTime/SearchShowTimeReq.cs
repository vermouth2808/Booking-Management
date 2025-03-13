using Core.Shared.Common.Models;

namespace Core.Shared.DTOs.Request.ShowTime
{
    public class SearchShowTimeReq : Search
    {
        public int? MovieId { get; set; }
        public int? RoomId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
