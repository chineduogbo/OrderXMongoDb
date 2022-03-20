namespace OrderXMongoWebApi.Models
{
    public class Stock
    {
        public string Id { get; set; }
        public string Name { get; set; }
         public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

}
