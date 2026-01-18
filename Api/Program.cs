using Api.Data;
using Api.Services;
using DotNetEnv;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Env.TraversePath().Load();
}

builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

if (FirebaseApp.DefaultInstance == null)
{
    var firebaseCredPath = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_PATH")
        ?? Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

    if (!string.IsNullOrWhiteSpace(firebaseCredPath) && File.Exists(firebaseCredPath))
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", firebaseCredPath);
    }

    var credential = GoogleCredential.GetApplicationDefault();

    FirebaseApp.Create(new AppOptions
    {
        Credential = credential
    });
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<FirebaseAuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.UseRouting();
app.MapControllers();

app.Run();