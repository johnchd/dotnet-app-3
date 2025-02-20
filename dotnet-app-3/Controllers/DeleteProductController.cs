using Microsoft.AspNetCore.Mvc;



// Accessible via: DELETE /api/DeleteProduct/<product>



namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteProductController : ControllerBase
    {
        [HttpDelete("{productName}")]
        public IActionResult DeleteProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest(new { message = "Product name is required" });
            }

            if (!AddProductController.Products.Remove(productName))
            {
                return NotFound(new { message = $"Product '{productName}' not found!" });
            }

            return Ok(new { message = $"Product '{productName}' deleted successfully!" });
        }
    }
}
