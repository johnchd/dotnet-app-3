using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        // Shared in-memory product list
        private static List<string> Products { get; } = new List<string>();

        public class ProductRequest
        {
            public string ProductName { get; set; }
        }

        // GET: /api/products (View Products)
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(Products);
        }

        // POST: /api/products (Add Product)
        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                return BadRequest(new { message = "Product name is required" });
            }

            Products.Add(request.ProductName);
            return Ok(new { message = $"Product '{request.ProductName}' added!" });
        }


        // DELETE: /api/products/{productName} (Delete Product)
        [HttpDelete("{productName}")]
        public IActionResult DeleteProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest(new { message = "Product name is required" });
            }

            if (!Products.Remove(productName))
            {
                return NotFound(new { message = $"Product '{productName}' not found!" });
            }

            return Ok(new { message = $"Product '{productName}' deleted successfully!" });
        }
    }
}
