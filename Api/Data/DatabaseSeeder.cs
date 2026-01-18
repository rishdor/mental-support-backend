using Api.Models;
using Bogus;

namespace Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Users.Any() || context.ContentItems.Any())
        {
            return;
        }

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FirebaseUid, f => $"firebase_{f.Random.AlphaNumeric(28)}")
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(1).ToUniversalTime());

        var users = userFaker.Generate(10);
        await context.Users.AddRangeAsync(users);

        var themes = new[]
        {
            ("Anxiety Relief", "Calm your racing thoughts and find peace in the present moment", "anxiety"),
            ("Better Sleep", "Gentle guidance to help you drift into restful sleep", "sleep"),
            ("Stress Management", "Learn to release tension and find balance in challenging times", "stress"),
            ("Confidence Building", "Strengthen your self-belief and inner voice", "confidence"),
            ("Mindfulness Practice", "Ground yourself in the here and now with guided awareness", "mindfulness"),
            ("Grief Support", "Navigate loss with compassion and understanding", "grief"),
            ("Anger Management", "Channel difficult emotions into healthy responses", "anger"),
            ("Depression Support", "Find light and hope during darker times", "depression"),
            ("Social Anxiety", "Build courage for social situations and connections", "social-anxiety"),
            ("Self-Compassion", "Treat yourself with the kindness you deserve", "self-compassion"),
            ("Morning Motivation", "Start your day with positive intention and energy", "motivation"),
            ("Panic Attack Relief", "Quick techniques to regain calm during intense moments", "panic"),
            ("Workplace Stress", "Navigate professional pressures with resilience", "work-stress"),
            ("Relationship Healing", "Process emotions around connections and boundaries", "relationships"),
            ("Body Acceptance", "Cultivate peace with your physical self", "body-image")
        };

        var emotionalTones = new[] { "firm", "soothing", "uplifting", "gentle", "reassuring" };

        var contentFaker = new Faker<ContentItem>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.Title, f => f.PickRandom(themes).Item1)
            .RuleFor(c => c.Description, (f, c) => 
            {
                var theme = themes.First(t => t.Item1 == c.Title);
                return theme.Item2;
            })
            .RuleFor(c => c.SituationTag, (f, c) => 
            {
                var theme = themes.First(t => t.Item1 == c.Title);
                return theme.Item3;
            })
            .RuleFor(c => c.IsPremium, f => f.Random.Bool(0.3f))
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(6).ToUniversalTime());

        var contentItems = new List<ContentItem>();
        foreach (var theme in themes)
        {
            var item = new ContentItem
            {
                Id = Guid.NewGuid(),
                Title = theme.Item1,
                Description = theme.Item2,
                SituationTag = theme.Item3,
                IsPremium = new Random().Next(100) < 30,
                CreatedAt = DateTime.UtcNow.AddDays(-new Random().Next(1, 180)),
                AudioVariants = new List<AudioVariant>()
            };

            var variantCount = new Random().Next(2, 5);
            for (int i = 0; i < variantCount; i++)
            {
                var tone = emotionalTones[new Random().Next(emotionalTones.Length)];
                var duration = new Random().Next(180, 1200);
                
                item.AudioVariants.Add(new AudioVariant
                {
                    Id = Guid.NewGuid(),
                    ContentItemId = item.Id,
                    EmotionalTone = tone,
                    AudioUrl = $"https://audio.example.com/{item.SituationTag}/{tone}-{Guid.NewGuid()}.mp3",
                    DurationSeconds = duration
                });
            }

            contentItems.Add(item);
        }

        await context.ContentItems.AddRangeAsync(contentItems);
        await context.SaveChangesAsync();
    }
}
