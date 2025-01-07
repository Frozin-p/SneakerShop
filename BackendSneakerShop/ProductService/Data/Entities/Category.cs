﻿namespace ProductService.Data.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
