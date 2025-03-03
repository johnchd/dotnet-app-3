using Microsoft.AspNetCore.Mvc;

[Route("api/feedback")]
[ApiController]
public class FeedbackController : ControllerBase
{
    [HttpGet("submit")]
    public IActionResult SubmitFeedback(string name, string comment)
    {
        string response = $"<h1>Thank you {name} for your feedback!</h1><p>{comment}</p>";
        return Content(response, "text/html"); // Returns raw HTML
    }
}
