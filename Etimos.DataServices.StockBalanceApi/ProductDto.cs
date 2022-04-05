namespace Etimos.DataServices.StockBalanceApi
{
    public class ProductDto
    {
        public ProductDto()
        {
            ProductId = Guid.NewGuid();
            StockAmount = 0;
            Created = DateTime.Now;
        }
        public Guid ProductId { get; set; }
        public int  StockAmount { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}