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
        // private readonly string _APIROOT;
        private readonly IConfiguration _configuration;

        private const string SecretHeader = "X-Secret-Key";
        private const string SecretValue = "test"; // Must match middleware

        public BFFProductController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_configuration["env"]}/api/products");
            request.Headers.Add(SecretHeader, SecretValue);

            var response = await _httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] object productRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_configuration["env"]}/api/products")
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
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_configuration["env"]}/api/products/{productName}");
            request.Headers.Add(SecretHeader, SecretValue);

            var response = await _httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }
    }
}
