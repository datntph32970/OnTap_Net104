namespace OnTap_Net104.Models
{
    public class Bill
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public decimal TotalBill { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual List<BillDetail>? BillDetails { get; set; }
        public virtual Account? Account { get; set; }
    }
}
