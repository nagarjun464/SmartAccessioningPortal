using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAccessioningPortal.Application.Models;
using SmartAccessioningPortal.Domain.Entities;
using SmartAccessioningPortal.Infrastructure.Data;

namespace SmartAccessioningPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CasesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE CASE
    [HttpPost]
    public async Task<IActionResult> CreateCase(CreateCaseRequest request)
    {
        var intakeCase = new Case
        {
            CreatedBy = request.CreatedBy,
            TestType = request.TestType,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow
        };

        _context.Cases.Add(intakeCase);
        await _context.SaveChangesAsync();

        return Ok(intakeCase);
    }

    // GET ALL CASES
    [HttpGet]
    public async Task<IActionResult> GetCases()
    {
        var cases = await _context.Cases
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(cases);
    }

    // GET CASE BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCaseById(int id)
    {
        var intakeCase = await _context.Cases
            .Include(x => x.Patient)
            .Include(x => x.KitInfo)
            .Include(x => x.Documents)
            .FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound();

        return Ok(intakeCase);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateCaseStatus(int id, [FromBody] string status)
    {
        var intakeCase = await _context.Cases.FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound("Case not found.");

        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");

        intakeCase.Status = status;

        await _context.SaveChangesAsync();

        return Ok(intakeCase);
    }

    // SAVE PATIENT
    [HttpPost("{id}/patient")]
    public async Task<IActionResult> SavePatient(int id, SavePatientRequest request)
    {
        var intakeCase = await _context.Cases
            .Include(x => x.Patient)
            .FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound("Case not found.");

        if (intakeCase.Patient == null)
        {
            intakeCase.Patient = new Patient
            {
                CaseId = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DOB = request.DOB,
                MRN = request.MRN
            };

            _context.Patients.Add(intakeCase.Patient);
        }
        else
        {
            intakeCase.Patient.FirstName = request.FirstName;
            intakeCase.Patient.LastName = request.LastName;
            intakeCase.Patient.DOB = request.DOB;
            intakeCase.Patient.MRN = request.MRN;
        }

        await _context.SaveChangesAsync();

        return Ok(intakeCase.Patient);
    }
    [HttpPost("{id}/documents")]
    public async Task<IActionResult> UploadDocument(int id, IFormFile file)
    {
        var intakeCase = await _context.Cases.FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound("Case not found.");

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var document = new Document
        {
            CaseId = id,
            FileName = file.FileName,
            FilePath = filePath,
            ContentType = file.ContentType,
            UploadedAt = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return Ok(document);
    }

    // SAVE KIT INFO
    [HttpPost("{id}/kit-info")]
    public async Task<IActionResult> SaveKitInfo(int id, SaveKitInfoRequest request)
    {
        var intakeCase = await _context.Cases
            .Include(x => x.KitInfo)
            .FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound("Case not found.");

        if (intakeCase.KitInfo == null)
        {
            intakeCase.KitInfo = new KitInfo
            {
                CaseId = id,
                KitBoxCode = request.KitBoxCode,
                LotCode = request.LotCode,
                OperatorName = request.OperatorName,
                ReceivedAt = request.ReceivedAt ?? DateTime.UtcNow
            };

            _context.KitInfos.Add(intakeCase.KitInfo);
        }
        else
        {
            intakeCase.KitInfo.KitBoxCode = request.KitBoxCode;
            intakeCase.KitInfo.LotCode = request.LotCode;
            intakeCase.KitInfo.OperatorName = request.OperatorName;
            intakeCase.KitInfo.ReceivedAt = request.ReceivedAt ?? intakeCase.KitInfo.ReceivedAt;
        }

        await _context.SaveChangesAsync();

        return Ok(intakeCase.KitInfo);
    }
}