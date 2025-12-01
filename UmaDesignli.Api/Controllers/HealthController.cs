using Microsoft.AspNetCore.Mvc;

namespace UmaDesignli.Api.Controllers
{
    /// <summary>
    /// Health check controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>OK if the service is healthy</returns>
        /// <response code="200">Service is healthy</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                service = "Uma Designli API",
                version = "1.0.0"
            });
        }
    }
}
