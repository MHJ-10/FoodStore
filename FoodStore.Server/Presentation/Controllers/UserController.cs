using FoodStore.Server.Application.Services;
using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Infrastructure.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Server.Presentation.Controllers
{
    //[Authorize]
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
        public async Task<ActionResult> ModelAsync(Register register)
        {
            var result = await _userService.RegisterAsync(register);
            return !result.StartsWith("Success:") ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequest tokenRequest)
        {
            var result = await _userService.GetTokenAsync(tokenRequest);
            return !result.IsAuthenticated? BadRequest(result): Ok(result);
        }

        [HttpPost("role-assignee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync(RoleAssignee roleAssignee)
        {
            var result = await _userService.AddRoleAsync(roleAssignee);
            return !result.StartsWith("Success:") ? BadRequest(result) : Ok(result);

        }
    }
}
