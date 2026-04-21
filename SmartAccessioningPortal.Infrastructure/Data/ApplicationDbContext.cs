using Microsoft.EntityFrameworkCore;
using SmartAccessioningPortal.Domain.Entities;

namespace SmartAccessioningPortal.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Case> Cases => Set<Case>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<KitInfo> KitInfos => Set<KitInfo>();
    public DbSet<Document> Documents => Set<Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Case>(entity =>
        {
            entity.HasKey(x => x.CaseId);
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();
            entity.Property(x => x.TestType).HasMaxLength(100);
            entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(x => x.PatientId);
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.MRN).HasMaxLength(100);

            entity.HasOne(x => x.Case)
                .WithOne(x => x.Patient)
                .HasForeignKey<Patient>(x => x.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<KitInfo>(entity =>
        {
            entity.HasKey(x => x.KitInfoId);
            entity.Property(x => x.KitBoxCode).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LotCode).HasMaxLength(100).IsRequired();
            entity.Property(x => x.OperatorName).HasMaxLength(100).IsRequired();

            entity.HasOne(x => x.Case)
                .WithOne(x => x.KitInfo)
                .HasForeignKey<KitInfo>(x => x.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(x => x.DocumentId);
            entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            entity.Property(x => x.ContentType).HasMaxLength(100).IsRequired();

            entity.HasOne(x => x.Case)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}