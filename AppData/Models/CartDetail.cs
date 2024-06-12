namespace AppData.Models
{
    public class CartDetail
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string CartID { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public decimal TransportFee { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Cart? Cart { get; set; }
    }
}
