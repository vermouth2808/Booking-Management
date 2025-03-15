namespace Core.Shared.DTOs.Response.ShowTime
{
    public class ShowTimeSearchRes
    {
        public int TotalRecords { get; set; }
        public IEnumerable<ShowTimeRes> ShowTimes { get; set; }
    }
}
