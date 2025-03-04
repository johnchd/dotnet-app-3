using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("bff/feedback")]
    public class BFFFeedbackController : ControllerBase
    {

        //  "APIROOT": "http://host.docker.internal:5250"


        private readonly HttpClient _httpClient;
        private readonly string _APIROOT;

        private const string SecretHeader = "X-Secret-Key";
        private const string SecretValue = "test"; // Must match middleware

        public BFFFeedbackController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _APIROOT = configuration["APIROOT"];
        }

        [HttpGet("submit")]
        public async Task<IActionResult> SubmitFeedback([FromQuery] string name, [FromQuery] string comment)
        {
            string backendUrl = $"{_APIROOT}/api/feedback/submit?name={Uri.EscapeDataString(name)}&comment={Uri.EscapeDataString(comment)}";

            var request = new HttpRequestMessage(HttpMethod.Get, backendUrl);
            request.Headers.Add(SecretHeader, SecretValue);

            var response = await _httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "text/html");
        }
    }
}
