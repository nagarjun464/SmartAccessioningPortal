using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAccessioningPortal.Api.Services;
using SmartAccessioningPortal.Application.Models;
using SmartAccessioningPortal.Application.Models.Responses;
using SmartAccessioningPortal.Domain.Entities;
using SmartAccessioningPortal.Infrastructure.Data;

namespace SmartAccessioningPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public CasesController(ApplicationDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

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

    [HttpGet]
    public async Task<IActionResult> GetCases()
    {
        var cases = await _context.Cases
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new CaseListResponse
            {
                CaseId = x.CaseId,
                Status = x.Status,
                TestType = x.TestType,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                PatientFirstName = x.Patient != null ? x.Patient.FirstName : null,
                PatientLastName = x.Patient != null ? x.Patient.LastName : null,
                KitBoxCode = x.KitInfo != null ? x.KitInfo.KitBoxCode : null,
                LotCode = x.KitInfo != null ? x.KitInfo.LotCode : null
            })
            .ToListAsync();

        return Ok(cases);
    }

    [HttpGet("documents/{documentId}/download")]
    public async Task<IActionResult> DownloadDocument(int documentId)
    {
        var document = await _context.Documents
            .FirstOrDefaultAsync(x => x.DocumentId == documentId);

        if (document == null)
            return NotFound("Document not found.");

        var download = await _fileStorage.OpenReadAsync(document.FilePath, document.ContentType);

        if (download is null)
            return NotFound("File not found on server.");

        return File(download.Stream, download.ContentType, document.FileName);
    }

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

        var storedFile = await _fileStorage.SaveAsync("documents", file);

        var document = new Document
        {
            CaseId = id,
            FileName = file.FileName,
            FilePath = storedFile.StoragePath,
            ContentType = file.ContentType,
            UploadedAt = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return Ok(document);
    }

    [HttpGet("{id}/tube-photos")]
    public async Task<IActionResult> GetTubePhotos(int id)
    {
        var intakeCase = await _context.Cases
            .FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound("Case not found.");

        var photos = await _context.TubePhotos
            .Where(x => x.CaseId == id)
            .OrderByDescending(x => x.CapturedAt)
            .Select(x => new TubePhotoResponse
            {
                TubePhotoId = x.TubePhotoId,
                FileName = x.FileName,
                ImageUrl = $"/api/cases/tube-photos/{x.TubePhotoId}/content",
                CapturedAt = x.CapturedAt
            })
            .ToListAsync();

        return Ok(photos);
    }

    [HttpGet("tube-photos/{tubePhotoId}/content")]
    public async Task<IActionResult> DownloadTubePhoto(int tubePhotoId)
    {
        var photo = await _context.TubePhotos
            .FirstOrDefaultAsync(x => x.TubePhotoId == tubePhotoId);

        if (photo == null)
            return NotFound("Tube photo not found.");

        var download = await _fileStorage.OpenReadAsync(photo.FilePath, photo.ContentType);

        if (download is null)
            return NotFound("File not found on server.");

        return File(download.Stream, download.ContentType, photo.FileName);
    }

    [HttpPost("{id}/tube-photos")]
    public async Task<IActionResult> UploadTubePhoto(int id, IFormFile file)
    {
        var intakeCase = await _context.Cases.FirstOrDefaultAsync(x => x.CaseId == id);

        if (intakeCase == null)
            return NotFound("Case not found.");

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (!file.ContentType.StartsWith("image/"))
            return BadRequest("Only image files are allowed for tube photos.");

        var storedFile = await _fileStorage.SaveAsync("tubephotos", file);

        var tubePhoto = new TubePhoto
        {
            CaseId = id,
            FileName = file.FileName,
            FilePath = storedFile.StoragePath,
            ContentType = file.ContentType,
            CapturedAt = DateTime.UtcNow
        };

        _context.TubePhotos.Add(tubePhoto);
        await _context.SaveChangesAsync();

        return Ok(tubePhoto);
    }

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
