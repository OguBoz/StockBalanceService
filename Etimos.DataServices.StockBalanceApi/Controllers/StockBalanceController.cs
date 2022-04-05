using Etimos.DataServices.StockBalanceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Etimos.DataServices.StockBalanceApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StockBalanceController : ControllerBase
    {
        private readonly ILogger<StockBalanceController> _logger;
        private readonly IStockBalanceService _stockBalanceService;

        public StockBalanceController(ILogger<StockBalanceController> logger, IStockBalanceService stockBalanceService)
        {
            _logger = logger;
            _stockBalanceService = stockBalanceService;
        }

        /// <summary>
        /// Returns list of all products and their details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of ProductDto</returns>
        [HttpGet("All")]
        public ActionResult<ICollection<ProductDto>> GetAll()
        {
            var products = _stockBalanceService.GetAll();
            return new OkObjectResult(products);
        }

        /// <summary>
        /// Returns a specific product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProductDto</returns>
        [HttpGet("{id}")]
        public ActionResult<ProductDto> Get(Guid id)
        {
            ProductDto product = _stockBalanceService.Get(id);

            if (product != null)
            {
                return new OkObjectResult(product);
            }

            return NotFound(id);
        }

        /// <summary>
        /// Returns available stock of a specific product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>StockBalance(int)</returns>
        [HttpGet("{id}/AvailableStock")]
        public ActionResult<int> GetAvailableStock(Guid id)
        {
            int availableStock = _stockBalanceService.GetAvailableStock(id);

            if (availableStock != -1)
            {
                return new OkObjectResult(availableStock);
            }

            return NotFound(id);
        }

        /// <summary>
        /// Decreases a specific product stock with specified amount.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns>ProductDto</returns>
        [HttpPut("{id}/Decrease/{amount}")]
        public ActionResult<ProductDto> Decrease(Guid id, int amount)
        {
            ProductDto product = _stockBalanceService.Decrease(id, amount);

            if (product != null)
            {
                _logger.LogInformation("Decreased stock of product with id: " + id + " by " + amount + "units.");
                return new OkObjectResult(product);
            }

            return NotFound(id);
        }

        /// <summary>
        /// Increases a specific product stock with specified amount.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns>ProductDto</returns>
        [HttpPut("{id}/Increase/{amount}")]
        public ActionResult<ProductDto> Increase(Guid id, int amount)
        {
            ProductDto product = _stockBalanceService.Increase(id, amount);

            if (product != null)
            {
                _logger.LogInformation("Increasing stock of product with id: " + id + " by " + amount + "units.");
                return new OkObjectResult(product);
            }

            return NotFound(id);
        }

        /// <summary>
        /// Creates a new product with given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ProductDto</returns>
        [HttpPost]
        public ActionResult<ProductDto> Create(string name)
        {
            _logger.LogInformation("Creating new product with name: " + name);

            ProductDto product = _stockBalanceService.Create(name);
            if (product != null)
            {
                return new OkObjectResult(product);
            }

            return NotFound(name);
        }
    }
}