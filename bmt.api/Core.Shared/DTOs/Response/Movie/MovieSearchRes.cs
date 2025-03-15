namespace Core.Shared.DTOs.Response.Movie
{
    public class MovieSearchRes
    {
        public int TotalRecords { get; set; }
        public IEnumerable<MovieRes> Movies { get; set; }
    }
}
