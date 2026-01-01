using FoodStore.Server.Application.Services;
using FoodStore.Server.Infrastructure.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Server.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IActionResult GetSecuredData()
        {
            return Ok("This Secured Data is available only for Authenticated Users.");
        }

        [HttpPost("Register")]
        public async Task<ActionResult> ModelAsync(Register register)
        {
            var result = await _userService.RegisterAsync(register);
            return Ok(result);
        }
    }
}
