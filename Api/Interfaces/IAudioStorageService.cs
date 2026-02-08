using Api.Contracts;

namespace Api.Interfaces;

public interface IAudioStorageService
{
    Task<AudioUploadResult> UploadAsync(IFormFile file);
}