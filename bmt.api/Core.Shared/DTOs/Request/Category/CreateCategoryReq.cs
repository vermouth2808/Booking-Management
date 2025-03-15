namespace Core.Shared.DTOs.Request.Category
{
    public class CreateCategoryReq
    {

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
