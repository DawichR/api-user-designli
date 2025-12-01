using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UmaDesignli.Application.Commands.Access;
using UmaDesignli.Application.Queries.Access;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Api.Controllers.Access
{
    /// <summary>
    /// Access controller - Handles authentication and user listing endpoints
    /// </summary>
    public class AccessController : BaseController
    {
        private readonly IMediator _mediator;

        public AccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

      
        /// <summary>
        /// User Login - Authenticates user and returns JWT token
        /// </summary>
        /// <param name="userapp">Login credentials (Userapp DTO with username and password)</param>
        /// <returns>JWT Token and username</returns>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="401">If the credentials are invalid</response>
        [HttpPost("user/login")]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Userapp userapp)
        {
            // Create command from Userapp DTO
            var command = new LoginCommand(userapp.Username, userapp.Password);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
