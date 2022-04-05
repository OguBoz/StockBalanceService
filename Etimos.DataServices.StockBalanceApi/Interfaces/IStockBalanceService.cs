namespace Etimos.DataServices.StockBalanceApi.Interfaces
{
    public interface IStockBalanceService
    {
        ICollection<ProductDto> GetAll();
        ProductDto Get(Guid id);
        int GetAvailableStock(Guid id);
        ProductDto Decrease(Guid id, int amount);
        ProductDto Increase(Guid id, int amount);
        ProductDto Create(string name);
    }
}