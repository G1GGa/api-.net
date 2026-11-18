using Microsoft.AspNetCore.Mvc;
using PhishingDetectorApi.Models;
using PhishingDetectorApi.Services;

namespace PhishingDetectorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    private readonly PhishingCheckerService _phishingCheckerService;

    public SecurityController(PhishingCheckerService phishingCheckerService)
    {
        _phishingCheckerService = phishingCheckerService;
    }

    [HttpPost("check")]
    public async Task<ActionResult<CheckResponse>> CheckUrl([FromBody] CheckRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Url))
        {
            return BadRequest(new { message = "URL cannot be empty." });
        }

        var result = await _phishingCheckerService.AnalyzeUrlAsync(request.Url);

        return Ok(result);
    }
}