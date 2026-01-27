using Api.Contracts;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.Interfaces;

namespace Api.Services;

public class ContentService : IContentService
{
    private readonly AppDbContext _context;

    public ContentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ContentItemResponse>> GetAllAsync(Guid userId)
    {
        return await _context.ContentItems
            .Select(ci => new ContentItemResponse
            {
                Id = ci.Id,
                Title = ci.Title,
                Description = ci.Description,
                SituationTag = ci.SituationTag,
                IsPremium = ci.IsPremium,
                AudioVariant = ci.AudioVariants
                    .Select(av => new AudioVariantResponse
                    {
                        Id = av.Id,
                        EmotionalTone = av.EmotionalTone,
                        AudioUrl = av.AudioUrl,
                        DurationSeconds = av.DurationSeconds
                    })
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    public async Task<ContentItemResponse?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.ContentItems
            .Where(ci => ci.Id == id)
            .Select(ci => new ContentItemResponse
            {
                Id = ci.Id,
                Title = ci.Title,
                Description = ci.Description,
                SituationTag = ci.SituationTag,
                IsPremium = ci.IsPremium,
                AudioVariant = ci.AudioVariants
                    .Select(av => new AudioVariantResponse
                    {
                        Id = av.Id,
                        EmotionalTone = av.EmotionalTone,
                        AudioUrl = av.AudioUrl,
                        DurationSeconds = av.DurationSeconds
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<AudioVariantResponse>> GetAudiosAsync(Guid contentItemId, Guid userId)
    {
        return await _context.AudioVariants
            .Where(av => av.ContentItemId == contentItemId)
            .Select(av => new AudioVariantResponse
            {
                Id = av.Id,
                EmotionalTone = av.EmotionalTone,
                AudioUrl = av.AudioUrl,
                DurationSeconds = av.DurationSeconds
            })
            .ToListAsync();
    }
}