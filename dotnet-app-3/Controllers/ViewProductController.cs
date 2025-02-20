using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

//Accessible via: GET /api/addproduct

namespace dotnet_app_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(AddProductController.Products);
        }
    }
}
