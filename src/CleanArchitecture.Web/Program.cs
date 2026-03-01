using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Web.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog Configuration (12-factor app: logging to stdout)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Application Services
// Use the assembly where our types are defined.
var applicationAssembly = typeof(IApplicationDbContext).Assembly;

builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(applicationAssembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// 3. Infrastructure Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Flashcards.db"));

builder.Services.AddScoped<IApplicationDbContext>(provider => 
    provider.GetRequiredService<ApplicationDbContext>());

// 4. Web API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Clean Architecture API";
        document.Info.Version = "v1";
        document.Info.Description = "API for Clean Architecture Flashcards Application";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// 5. Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Clean Architecture API v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 6. Seed Data
try 
{
    await ApplicationDbContextSeed.SeedSampleDataAsync(app.Services);
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred during database seeding.");
}

try
{
    Log.Information("Starting web host in {Environment} environment", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Partial class for integration tests if needed
public partial class Program { }