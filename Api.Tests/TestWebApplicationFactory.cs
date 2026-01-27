using System.Net.Http;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("DISABLE_AUTH", "true");
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", "InMemory");

        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DISABLE_AUTH"] = "true",
                ["ConnectionStrings:DefaultConnection"] = "InMemory"
            });
        });

        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Seed(db);
        });
    }

    private static void Seed(AppDbContext db)
    {
        if (!db.ContentItems.Any())
        {
            var contentId = Guid.NewGuid();
            db.ContentItems.Add(new ContentItem
            {
                Id = contentId,
                Title = "Test Content",
                Description = "Test Description",
                SituationTag = "test",
                IsPremium = false,
                CreatedAt = DateTime.UtcNow,
                AudioVariants =
                {
                    new AudioVariant
                    {
                        Id = Guid.NewGuid(),
                        ContentItemId = contentId,
                        EmotionalTone = "soothing",
                        AudioUrl = "https://example.com/audio.mp3",
                        DurationSeconds = 60
                    }
                }
            });
        }

        db.SaveChanges();
    }
}
