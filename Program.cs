using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;
using zenBeat.Data;
using zenBeat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<ZenBeatDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IStepService, StepService>();

// Force Kestrel to listen on IPv4 0.0.0.0:8080
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 8080);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ZenBeatDbContext>();
    var dbSource = context.Database.GetDbConnection().DataSource;
    var dir = Path.GetDirectoryName(dbSource);
    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
    {
        Directory.CreateDirectory(dir);
    }

    context.Database.EnsureCreated();
    await DataSeeder.SeedData(context);
}

app.Run();