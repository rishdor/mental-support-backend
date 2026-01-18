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

    public ContentItemResponse GetContentItemById(Guid id)
    {
        var contentItem = _dbContext.ContentItems
            .Where(ci => ci.Id == id)
            .Select(ci => new ContentItemResponse
            {
                Id = ci.Id,
                Title = ci.Title,
                Description = ci.Description,
                SituationTag = ci.SituationTag,
                IsPremium = ci.IsPremium
            })
            .FirstOrDefault();

        if (contentItem == null)
        {
            throw new KeyNotFoundException("Content item not found.");
        }

        return contentItem;
    }
}