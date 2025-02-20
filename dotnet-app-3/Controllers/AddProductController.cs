using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


// Add Products and store  in memory
// Validates input (prevents empty names)
// Returns JSON success/error responses
// Accessible via: POST /api/addproduct

namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddProductController : ControllerBase
    {
        // Make products list PUBLIC and STATIC so it can be accessed from other controllers
        public static List<string> Products { get; } = new List<string>();

        public class ProductRequest
        {
            public string ProductName { get; set; }
        }

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
    }
}



