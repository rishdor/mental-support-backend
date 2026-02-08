namespace Api.Contracts;

public record AudioUploadResult(
    string BlobUrl,
    int DurationSeconds
);