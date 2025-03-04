using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("bff/products")]
    public class BFFProductController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _APIROOT;

        private const string SecretHeader = "X-Secret-Key";
        private const string SecretValue = "test"; // Must match middleware

        public BFFProductController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _APIROOT = configuration["APIROOT"];
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_APIROOT}/api/products");
            request.Headers.Add(SecretHeader, SecretValue);

            var response = await _httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] object productRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_APIROOT}/api/products")
            {
                Content = JsonContent.Create(productRequest)
            };
            request.Headers.Add(SecretHeader, SecretValue);

            var response = await _httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        [HttpDelete("{productName}")]
        public async Task<IActionResult> DeleteProduct(string productName)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_APIROOT}/api/products/{productName}");
            request.Headers.Add(SecretHeader, SecretValue);

            var response = await _httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }
    }
}
