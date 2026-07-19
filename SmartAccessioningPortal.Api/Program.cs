using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SmartAccessioningPortal.Api.Services;
using SmartAccessioningPortal.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DemoDataSeeder>();

var databaseProvider = builder.Configuration["Database:Provider"];
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var useSqlServer =
    builder.Environment.IsDevelopment() ||
    string.Equals(databaseProvider, "SqlServer", StringComparison.OrdinalIgnoreCase);

if (useSqlServer && !string.IsNullOrWhiteSpace(defaultConnection))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(defaultConnection));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("CloudRunDb"));
}

var storageProvider = builder.Configuration["Storage:Provider"];
var useGcpStorage =
    string.Equals(storageProvider, "Gcp", StringComparison.OrdinalIgnoreCase) ||
    (!builder.Environment.IsDevelopment() &&
     !string.IsNullOrWhiteSpace(builder.Configuration["GcpStorage:BucketName"]));

if (useGcpStorage)
{
    builder.Services.AddSingleton<IFileStorageService, GcpStorageService>();
}
else
{
    builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var uploadsPath = Path.Combine(Path.GetTempPath(), "Uploads");
Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

await InitializeDatabaseAsync(app);

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (context.Database.IsRelational() &&
        configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
    {
        await context.Database.MigrateAsync();
    }
    else if (!context.Database.IsRelational())
    {
        await context.Database.EnsureCreatedAsync();
    }

    if (configuration.GetValue<bool>("DemoSeed:Enabled"))
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DemoDataSeeder>();
        var patientCount = configuration.GetValue("DemoSeed:PatientCount", 25);
        var caseCount = configuration.GetValue("DemoSeed:CaseCount", 40);
        var force = configuration.GetValue<bool>("DemoSeed:Force");

        await seeder.SeedAsync(patientCount, caseCount, force);
    }
}
