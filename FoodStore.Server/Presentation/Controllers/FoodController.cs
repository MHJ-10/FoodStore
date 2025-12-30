using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Server.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoodController : ControllerBase
{
    [HttpGet]
    public IActionResult GetFoods()
    {
        return Ok();
    }
}
