using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MyNewwRedis.Models;
using Newtonsoft.Json;
using MyNewwRedis.Data;
using System.Text;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContextClass _dbContextClass;
        private readonly IDistributedCache _distributedCache;

        public ProductController(DbContextClass dbContextClass, IDistributedCache distributedCache)
        {
            _dbContextClass = dbContextClass;
            _distributedCache = distributedCache;
        }


        [HttpGet]
        public async Task<List<Product>> GetProductsAsync()
        {
            var productsByteArray = await _distributedCache.GetAsync("Product");
            if (productsByteArray != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(productsByteArray);
                var productList = JsonConvert.DeserializeObject<List<Product>>(cachedDataString);
                return productList;
            }
            else
            {
                var products = await _dbContextClass.Products.ToListAsync();
                string serializedProductsLists = JsonConvert.SerializeObject(products);
                await _distributedCache.SetAsync("Product", Encoding.UTF8.GetBytes(serializedProductsLists));
                return products;
            }
        }

        [HttpPost]
        public async Task<Product> AddProductAsync(Product product)
        {
            _dbContextClass.Add(product);
            await _dbContextClass.SaveChangesAsync();
            await _distributedCache.RemoveAsync("Product");
            return product;
        }


    }

}
