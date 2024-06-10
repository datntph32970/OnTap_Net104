using System;
using System.Collections.Generic;

namespace AppData.Models;

public partial class BillDetail
{
    public Guid Id { get; set; }

    public string BillId { get; set; } = null!;

    public Guid ProductId { get; set; }

    public decimal ProductPrice { get; set; }

    public int Quantity { get; set; }

    public int Status { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
