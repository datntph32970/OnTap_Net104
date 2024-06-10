using System;
using System.Collections.Generic;

namespace AppData.Models;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public int Status { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
}
