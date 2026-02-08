using Api.Contracts;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.Interfaces;

namespace Api.Services;

public class ContentQueryService : IContentQueryService
{
    private readonly AppDbContext _context;

    public ContentQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ContentItemResponse>> GetAllAsync(Guid userId)
    {
        return await _context.ContentItems
            .Where(ci => ci.IsPremium == false) // for demo simplicity, only non-premium content is returned
            .Select(ci => new ContentItemResponse(
                ci.Id,
                ci.Title,
                ci.Description,
                ci.SituationTag,
                ci.IsPremium,
                ci.AudioVariants
                    .Select(av => new AudioVariantResponse(
                        av.Id,
                        av.EmotionalTone,
                        av.AudioUrl,
                        av.DurationSeconds
                    ))
                    .FirstOrDefault()
            ))
            .ToListAsync();
    }

    public async Task<ContentItemResponse?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.ContentItems
            .Where(ci => ci.Id == id)
            .Select(ci => new ContentItemResponse(
                ci.Id,
                ci.Title,
                ci.Description,
                ci.SituationTag,
                ci.IsPremium,
                ci.AudioVariants
                    .Select(av => new AudioVariantResponse(
                        av.Id,
                        av.EmotionalTone,
                        av.AudioUrl,
                        av.DurationSeconds
                    ))
                    .FirstOrDefault()
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<AudioVariantResponse>> GetAudiosAsync(Guid contentItemId, Guid userId)
    {
        return await _context.AudioVariants
            .Where(av => av.ContentItemId == contentItemId)
            .Select(av => new AudioVariantResponse(
                av.Id,
                av.EmotionalTone,
                av.AudioUrl,
                av.DurationSeconds
            ))
            .ToListAsync();
    }
}