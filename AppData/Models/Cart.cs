using System;
using System.Collections.Generic;

namespace AppData.Models;

public partial class Cart
{
    public string Username { get; set; } = null!;

    public int? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Account UsernameNavigation { get; set; } = null!;
}
