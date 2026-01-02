using FoodStore.Server.Application.Services;
using FoodStore.Server.Application.Users.Commands;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Infrastructure.DataModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Server.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSecuredData()
        {
            return Ok("This Secured Data is available only for Authenticated Users.");
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterUser.Request registerRequest)
        {
            var result = await _mediator.Send(registerRequest);
            return result.Match<ActionResult>(
          // SUCCESS
          response => Ok(new RegisterUser.Response
          {
              UserId = response.UserId,
              UserName = response.UserName,
              Email = response.Email
          }),

          // ERROR
          errors => BadRequest(errors)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUser.Request loginRequest)
        {
            var result = await _mediator.Send(loginRequest);
            return result.Match<ActionResult>(
                // SUCCESS: construct response using the constructor that accepts tokens
                response => Ok(new LoginUser.Response(response.AcessToken, response.RefreshToken)),

                // ERROR
                errors => BadRequest(errors)
            );
        }

        [HttpPost("add-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync(AddRole.Request addRoleRequest)
        {
            var result = await _mediator.Send(addRoleRequest);
            return result.Match<ActionResult>(
           // SUCCESS: construct response using the constructor that accepts tokens
           response => Ok(),
           // ERROR
           errors => BadRequest(errors)
           );
        }
        [Authorize]
        [HttpDelete("revoke-refresh-tokens/{userId}")]
        public async Task<IActionResult> RevokeUserRefreshTokens(RevokeRefreshTokens.Request request)
        {
            var result = await _mediator.Send(request);

            return result.Match<ActionResult>(
                // SUCCESS
                _ => Ok(new { message = "Refresh tokens revoked successfully." }),

                // ERROR
                errors => BadRequest(errors)
            );
        }
        [HttpPost("login/refresh-token")]
        public async Task<IActionResult> LoginUserWithRefreshToken(LoginUserWithRefreshToken.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Match<ActionResult>(
                     // SUCCESS CASE
                     response => Ok( new LoginUser.Response(response.AccessToken, response.RefreshToken)),

                     // ERROR CASE
                     errors => BadRequest(errors)
                 );
        }
    }
}
