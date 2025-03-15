using Core.Shared.Common.Models;

namespace Core.Shared.DTOs.Request.Movie
{
    public class SearchMovieReq : Search
    {
        public  DateTime FromDate { get; set; } 
        public  DateTime ToDate { get; set; } 
    }
}
