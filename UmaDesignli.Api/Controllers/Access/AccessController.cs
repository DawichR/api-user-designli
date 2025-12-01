using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UmaDesignli.Application.Commands.Access;

namespace UmaDesignli.Api.Controllers.Access
{
    /// <summary>
    /// Access controller.
    /// </summary>
    public class AccessController : BaseController
    {
        private readonly IMediator _mediator;

        public AccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ///// <summary>
        ///// Get all employees
        ///// </summary>
        ///// <returns>List of all employees</returns>
        ///// <response code="200">Returns the list of employees</response>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetAll()
        //{
        //    var result = await _mediator.Send(new GetAllEmployeesQuery());
        //    return Ok(result);
        //}

        /// <summary>
        /// User Login.
        /// </summary>
        /// <param name="command">Login credentials</param>
        /// <returns>JWT Token</returns>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="401">If the credentials are invalid</response>
        [HttpPost("user/login")]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
