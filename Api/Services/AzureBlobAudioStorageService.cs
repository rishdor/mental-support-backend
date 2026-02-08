using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Api.Contracts;
using Api.Interfaces;

namespace Api.Services;

public class AzureBlobAudioStorageService : IAudioStorageService
{
    private readonly BlobContainerClient _container;

    public AzureBlobAudioStorageService(IConfiguration config)
    {
        var connectionString = config["AzureBlob:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("AzureBlob:ConnectionString is missing.");

        var containerName = config["AzureBlob:ContainerName"];
        if (string.IsNullOrWhiteSpace(containerName))
            throw new InvalidOperationException("AzureBlob:ContainerName is missing.");

        _container = new BlobContainerClient(connectionString, containerName);
        _container.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<AudioUploadResult> UploadAsync(IFormFile file)
    {
        if (file.Length == 0)
            throw new InvalidOperationException("Empty file.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension is not (".mp3" or ".wav" or ".m4a"))
            throw new InvalidOperationException("Unsupported audio format.");

        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var tagFile = TagLib.File.Create(
            new TagLibStreamFileAbstraction(
                file.FileName,
                memoryStream,
                memoryStream
            )
        );

        var durationSeconds = (int)Math.Round(
            tagFile.Properties.Duration.TotalSeconds
        );

        if (durationSeconds <= 0 || durationSeconds > 60 * 60 * 2)
            throw new InvalidOperationException("Invalid audio duration.");

        memoryStream.Position = 0;

        var blobName = $"audio/{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid()}{extension}";

        var blobClient = _container.GetBlobClient(blobName);

        await blobClient.UploadAsync(
            memoryStream,
            new BlobHttpHeaders { ContentType = file.ContentType }
        );

        return new AudioUploadResult(
            blobClient.Uri.ToString(),
            durationSeconds
        );
    }
}
