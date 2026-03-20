namespace AspNetPractice25.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
