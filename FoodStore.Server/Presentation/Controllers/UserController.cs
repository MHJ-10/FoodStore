using FoodStore.Server.Application.Services;
using FoodStore.Server.Infrastructure.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Server.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSecuredData()
        {
            return Ok("This Secured Data is available only for Authenticated Users.");
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync(Register registerRequest)
        {
            var result = await _userService.RegisterAsync(registerRequest);
            return !result.IsAuthenticated ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(Login loginRequest)
        {
            var result = await _userService.LoginAsync(loginRequest);
            return !result.IsAuthenticated? BadRequest(result): Ok(result);
        }

        [HttpPost("Add-Role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync(AddRole addRoleRequest)
        {
            var result = await _userService.AddRoleAsync(addRoleRequest);
            return !result.StartsWith("Success:") ? BadRequest(result) : Ok(result);
        }
    }
}
