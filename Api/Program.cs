using Api.Data;
using Api.Middleware;
using Api.Services;
using Api.Interfaces;
using DotNetEnv;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Env.TraversePath().Load();
}

builder.Configuration.AddEnvironmentVariables();

var disableAuth = builder.Configuration.GetValue<bool>("DISABLE_AUTH");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!builder.Environment.IsEnvironment("Testing") && string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is missing or empty.");
}

connectionString ??= builder.Environment.IsEnvironment("Testing")
    ? "Host=localhost;Database=placeholder;Username=placeholder;Password=placeholder"
    : string.Empty;

if (FirebaseApp.DefaultInstance == null)
{
    if (disableAuth)
    {
        if (builder.Environment.IsProduction())
        {
            throw new InvalidOperationException("DISABLE_AUTH cannot be enabled in Production.");
        }
    }

    var firebaseCredPath = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_PATH")
        ?? Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

    if (!string.IsNullOrWhiteSpace(firebaseCredPath) && File.Exists(firebaseCredPath))
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", firebaseCredPath);
        var credential = GoogleCredential.GetApplicationDefault();

        FirebaseApp.Create(new AppOptions
        {
            Credential = credential
        });
    }
    else if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Testing") || disableAuth)
    {
        Console.WriteLine("Firebase credentials not found. Running in development mode without Firebase authentication.");
    }
    else
    {
        throw new InvalidOperationException("Firebase credentials are required in production.");
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Testing"))
    {
        options.UseInMemoryDatabase("ApiTesting");
    }
    else
    {
        options.UseNpgsql(connectionString);
    }
});

builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IUserResolutionService, UserResolutionService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IOnboardingService, OnboardingService>();

builder.Services.AddHttpLogging(opts =>
{
    opts.LoggingFields = HttpLoggingFields.RequestPath |
                         HttpLoggingFields.RequestMethod |
                         HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Logger.LogInformation("API started in {Environment}", app.Environment.EnvironmentName);
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("GlobalException");
        if (exFeature?.Error != null)
        {
            logger.LogError(exFeature.Error, "Unhandled exception at {Path}", exFeature.Path);
        }
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
    });
});

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Database not available at startup; continuing for demo mode.");
    }
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpLogging();
app.UseRouting();

if (disableAuth)
{
    app.Use(async (context, next) =>
    {
        context.Items["FirebaseUid"] = "test_uid";
        context.Items["FirebaseEmail"] = "test@example.com";
        await next();
    });
}
else
{
    app.UseMiddleware<FirebaseAuthMiddleware>();
}

app.MapControllers();

app.Run();