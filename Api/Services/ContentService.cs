using Api.Contracts;
using Api.Data;

namespace Api.Services;

public class ContentService
{
    private readonly AppDbContext _dbContext;

    public ContentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<ContentItemResponse> GetAllContentItems()
    {
        return _dbContext.ContentItems
            .Select(ci => new ContentItemResponse
            {
                Id = ci.Id,
                Title = ci.Title,
                Description = ci.Description,
                SituationTag = ci.SituationTag,
                IsPremium = ci.IsPremium
            })
            .ToList();
    }

    public ContentItemDetails GetContentItemById(Guid id)
    {
        var contentItem = _dbContext.ContentItems
            .Where(ci => ci.Id == id)
            .Select(ci => new ContentItemDetails
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
            .FirstOrDefault();

        if (contentItem == null || contentItem.AudioVariant == null)
        {
            throw new KeyNotFoundException("Content item not found.");
        }

        return contentItem;
    }

    public IEnumerable<AudioVariantResponse> GetAudioVariantsByContentItemId(Guid contentItemId)
    {
        var contentItem = _dbContext.ContentItems
            .FirstOrDefault(ci => ci.Id == contentItemId);

        if (contentItem == null)
        {
            throw new KeyNotFoundException("Content item not found.");
        }

        return _dbContext.AudioVariants
            .Where(av => av.ContentItemId == contentItemId)
            .Select(av => new AudioVariantResponse
            {
                Id = av.Id,
                EmotionalTone = av.EmotionalTone,
                AudioUrl = av.AudioUrl,
                DurationSeconds = av.DurationSeconds
            })
            .ToList();
    }
}