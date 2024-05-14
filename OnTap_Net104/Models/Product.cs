namespace OnTap_Net104.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        // Quan hệ - Navigation
        public virtual List<BillDetail>? BillDetails { get; set; }
        public virtual List<CartDetail>? CartDetails { get; set; }
    }
}
