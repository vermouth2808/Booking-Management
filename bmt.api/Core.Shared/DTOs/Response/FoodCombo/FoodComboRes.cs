﻿namespace Core.Shared.DTOs.Response.FoodCombo
{
    public class FoodComboRes
    {
        public int ComboId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
