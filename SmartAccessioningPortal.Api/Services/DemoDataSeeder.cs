using Microsoft.EntityFrameworkCore;
using SmartAccessioningPortal.Domain.Entities;
using SmartAccessioningPortal.Infrastructure.Data;

namespace SmartAccessioningPortal.Api.Services;

public sealed class DemoDataSeeder
{
    private const string DemoCreatedBy = "Demo Operator";
    private readonly ApplicationDbContext _context;

    public DemoDataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DemoSeedResult> SeedAsync(
        int patientCount = 25,
        int caseCount = 40,
        bool force = false,
        CancellationToken cancellationToken = default)
    {
        var existingDemoCases = await _context.Cases
            .CountAsync(x => x.CreatedBy == DemoCreatedBy, cancellationToken);

        if (existingDemoCases > 0 && !force)
        {
            return new DemoSeedResult(existingDemoCases, 0, false);
        }

        if (force)
        {
            var demoCases = await _context.Cases
                .Include(x => x.Patient)
                .Include(x => x.KitInfo)
                .Where(x => x.CreatedBy == DemoCreatedBy)
                .ToListAsync(cancellationToken);

            _context.Cases.RemoveRange(demoCases);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var patients = DemoPatients.Take(Math.Clamp(patientCount, 1, DemoPatients.Length)).ToArray();
        var tests = new[] { "Panorama", "Signatera", "Empower" };
        var statuses = new[] { "ReadyForReview", "ReadyForReview", "Draft" };
        var now = DateTime.UtcNow;

        for (var i = 0; i < caseCount; i++)
        {
            var patient = patients[i % patients.Length];
            var testType = tests[i % tests.Length];
            var caseNumber = i + 1;

            var intakeCase = new Case
            {
                CreatedBy = DemoCreatedBy,
                TestType = testType,
                Status = statuses[i % statuses.Length],
                CreatedAt = now.AddHours(-caseNumber * 3),
                Patient = new Patient
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    DOB = patient.Dob,
                    MRN = $"DEMO-{10000 + i}"
                },
                KitInfo = new KitInfo
                {
                    KitBoxCode = $"{testType[..1].ToUpperInvariant()}KIT-{2026000 + caseNumber}",
                    LotCode = $"LOT-{7000 + i}",
                    OperatorName = DemoCreatedBy,
                    ReceivedAt = now.AddHours(-caseNumber * 3).AddMinutes(12)
                }
            };

            _context.Cases.Add(intakeCase);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new DemoSeedResult(caseCount, caseCount, true);
    }

    private static readonly DemoPatient[] DemoPatients =
    [
        new("Avery", "Stone", new DateTime(1989, 3, 14)),
        new("Maya", "Patel", new DateTime(1992, 7, 22)),
        new("Jordan", "Reed", new DateTime(1984, 1, 9)),
        new("Elena", "Morris", new DateTime(1995, 11, 3)),
        new("Noah", "Bennett", new DateTime(1978, 5, 18)),
        new("Priya", "Shah", new DateTime(1990, 9, 28)),
        new("Lucas", "Carter", new DateTime(1986, 12, 7)),
        new("Sofia", "Nguyen", new DateTime(1993, 2, 16)),
        new("Ethan", "Brooks", new DateTime(1981, 8, 30)),
        new("Isabella", "Rivera", new DateTime(1996, 4, 25)),
        new("Amelia", "Kim", new DateTime(1988, 10, 12)),
        new("Liam", "Cooper", new DateTime(1983, 6, 5)),
        new("Zoe", "Collins", new DateTime(1991, 1, 27)),
        new("Mason", "Foster", new DateTime(1979, 9, 2)),
        new("Nora", "Hayes", new DateTime(1994, 3, 19)),
        new("Aria", "Wright", new DateTime(1987, 7, 6)),
        new("Caleb", "Torres", new DateTime(1982, 12, 21)),
        new("Mila", "Parker", new DateTime(1997, 5, 8)),
        new("Henry", "Adams", new DateTime(1985, 2, 11)),
        new("Leah", "Mitchell", new DateTime(1990, 6, 24)),
        new("Owen", "Turner", new DateTime(1980, 11, 15)),
        new("Chloe", "Ramirez", new DateTime(1998, 8, 4)),
        new("James", "Nelson", new DateTime(1986, 4, 17)),
        new("Grace", "Morgan", new DateTime(1992, 10, 29)),
        new("Ryan", "Bailey", new DateTime(1984, 5, 31))
    ];

    private sealed record DemoPatient(string FirstName, string LastName, DateTime Dob);
}

public sealed record DemoSeedResult(int ExistingOrTotalCases, int CreatedCases, bool Seeded);
