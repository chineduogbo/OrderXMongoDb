namespace OrderXMongoWebApi.Models
{
    public class OrderItem
    {
        public string StockName { get; set; }
        public string Quantity { get; set; }
        public string Orderdescription { get; set; }
        public string Price { get; set; }
        public string DateOrdered { get; set; }
    }

}
