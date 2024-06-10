using System;
using System.Collections.Generic;

namespace AppData.Models;

public partial class CartDetail
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string CartId { get; set; } = null!;

    public int Quantity { get; set; }

    public bool Status { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
