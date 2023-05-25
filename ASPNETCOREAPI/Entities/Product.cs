using System;
using System.Collections.Generic;

namespace ASPNETCOREAPI.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Thumbnail { get; set; }

    public int Qty { get; set; }

    public int CategoryId { get; set; }

    public int? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Cart>? Carts { get; set; }

    public virtual Category? Category { get; set; }
}
