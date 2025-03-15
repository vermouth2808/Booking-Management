﻿namespace Core.Shared.DTOs.Response.Category
{
    public class CategoryRes
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
