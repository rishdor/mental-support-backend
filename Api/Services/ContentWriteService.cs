using Api.Data;
using Api.Models;
using Api.Interfaces;
using Api.Contracts;
using Microsoft.EntityFrameworkCore;

public class ContentWriteService : IContentWriteService
{
    private readonly AppDbContext _context;

    public ContentWriteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(CreateContentRequest request)
    {
        var content = new ContentItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            SituationTag = request.SituationTag,
            IsPremium = request.IsPremium,
            CreatedAt = DateTime.UtcNow,
            AudioVariants =
            {
                new AudioVariant
                {
                    Id = Guid.NewGuid(),
                    AudioUrl = request.AudioUrl,
                    DurationSeconds = request.DurationSeconds,
                    EmotionalTone = request.EmotionalTone
                }
            }
        };

        _context.ContentItems.Add(content);
        await _context.SaveChangesAsync();

        return content.Id;
    }

    public async Task AddAudioVariantAsync(
        Guid contentItemId,
        CreateAudioVariantRequest request)
    {
        var content = await _context.ContentItems
            .Include(c => c.AudioVariants)
            .SingleOrDefaultAsync(c => c.Id == contentItemId);

        if (content == null)
            throw new InvalidOperationException("Content item not found.");

        content.AudioVariants.Add(new AudioVariant
        {
            Id = Guid.NewGuid(),
            ContentItemId = contentItemId,
            AudioUrl = request.AudioUrl,
            DurationSeconds = request.DurationSeconds,
            EmotionalTone = request.EmotionalTone
        });

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid contentItemId)
    {
        var content = await _context.ContentItems.FindAsync(contentItemId);

        if (content == null)
            return;

        _context.ContentItems.Remove(content);
        await _context.SaveChangesAsync();
    }
}