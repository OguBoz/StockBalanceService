using Etimos.DataServices.StockBalanceApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;

namespace Etimos.DataServices.StockBalanceApi.Services
{
    public class StockBalanceService : IStockBalanceService
    {
        private readonly IMemoryCache _memoryCache;

        public StockBalanceService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public ProductDto Create(string name)
        {
            ProductDto newProduct = new ProductDto() { Name = name };
            _memoryCache.Set<ProductDto>(newProduct.ProductId, newProduct, new MemoryCacheEntryOptions().SetSize(1));

            return newProduct;
        }

        public ProductDto Decrease(Guid id, int amount)
        {
            ProductDto product;

            if (_memoryCache.TryGetValue<ProductDto>(id, out product))
            {
                if (product.StockAmount - amount >= 0)
                    product.StockAmount -= amount;
                else
                    return null;

                _memoryCache.Set<ProductDto>(id, product);
                return product;
            }
            return null;
        }

        public ProductDto Get(Guid id)
        {
            ProductDto product;

            if (_memoryCache.TryGetValue<ProductDto>(id, out product))
            {
                return product;
            }

            return null;
        }

        public ICollection<ProductDto> GetAll()
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_memoryCache) as ICollection;

            var products = new List<ProductDto>();
            if (collection != null)
                foreach (var product in collection)
                {
                    var methodInfo = product.GetType().GetProperty("Key");
                    //var val = methodInfo.GetValue(item.);
                    var x = product.GetType().GetProperty("Value").GetValue(product);
                    var z = x.GetType().GetProperty("Value").GetValue(x);
                    products.Add((ProductDto)z);
                }
            return products;
        }

        public int GetAvailableStock(Guid id)
        {
            ProductDto product;

            if (_memoryCache.TryGetValue<ProductDto>(id, out product))
            {
                return product.StockAmount;
            }

            return -1;
        }

        public ProductDto Increase(Guid id, int amount)
        {
            ProductDto product;

            if (_memoryCache.TryGetValue<ProductDto>(id, out product))
            {
                product.StockAmount += amount;
                _memoryCache.Set<ProductDto>(id, product);
                return product;
            }
            return null;
        }
    }
}
