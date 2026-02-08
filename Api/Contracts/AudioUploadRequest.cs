namespace Api.Contracts;

public record AudioUploadRequest(
    IFormFile File
);