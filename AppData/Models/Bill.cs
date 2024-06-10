using System;
using System.Collections.Generic;

namespace AppData.Models;

public partial class Bill
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int Status { get; set; }

    public DateTime CreateDate { get; set; }

    public decimal TotalBill { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual Account UsernameNavigation { get; set; } = null!;
}
