using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("bff/products")]
    public class BffController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public BffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: /bff/products (Fetch all products)
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _httpClient.GetAsync("http://localhost:5250/api/products");
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        // POST: /bff/products (Add a product)
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] object productRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5250/api/products", productRequest);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        // DELETE: /bff/products/{productName} (Delete a product)
        [HttpDelete("{productName}")]
        public async Task<IActionResult> DeleteProduct(string productName)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5250/api/products/{productName}");
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }
    }
}
