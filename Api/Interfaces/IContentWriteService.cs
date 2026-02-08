using Api.Contracts;

namespace Api.Interfaces;

public interface IContentWriteService
{
    Task<Guid> CreateAsync(CreateContentRequest request);
    Task AddAudioVariantAsync(Guid contentItemId, CreateAudioVariantRequest request);
    Task DeleteAsync(Guid contentItemId);
}