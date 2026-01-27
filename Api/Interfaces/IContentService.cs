using Api.Contracts;

namespace Api.Interfaces;

public interface IContentService
{
    Task<List<ContentItemResponse>> GetAllAsync(Guid userId);
    Task<ContentItemResponse?> GetByIdAsync(Guid id, Guid userId);
    Task<List<AudioVariantResponse>> GetAudiosAsync(Guid contentItemId, Guid userId);
}