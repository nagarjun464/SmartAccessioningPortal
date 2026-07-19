using Microsoft.AspNetCore.Mvc;
using SmartAccessioningPortal.Api.Services;

namespace SmartAccessioningPortal.Api.Controllers;

[ApiController]
[Route("api/demo-data")]
public class DemoDataController : ControllerBase
{
    private readonly DemoDataSeeder _seeder;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public DemoDataController(
        DemoDataSeeder seeder,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _seeder = seeder;
        _configuration = configuration;
        _environment = environment;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed(
        [FromQuery] int patientCount = 25,
        [FromQuery] int caseCount = 40,
        [FromQuery] bool force = false,
        CancellationToken cancellationToken = default)
    {
        var endpointAllowed =
            _environment.IsDevelopment() ||
            _configuration.GetValue<bool>("DemoSeed:AllowEndpoint");

        if (!endpointAllowed)
        {
            return NotFound();
        }

        var result = await _seeder.SeedAsync(patientCount, caseCount, force, cancellationToken);

        return Ok(new
        {
            result.Seeded,
            result.CreatedCases,
            TotalDemoCases = result.ExistingOrTotalCases,
            PatientProfiles = patientCount
        });
    }
}
