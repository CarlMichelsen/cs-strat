using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Healthcheck controller. Used to check the health of the image.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Healthcheck endpoint.
    /// </summary>
    /// <returns>Nothing with status 200.</returns>
    [HttpGet]
    public ActionResult Get()
    {
        return this.Ok();
    }
}
