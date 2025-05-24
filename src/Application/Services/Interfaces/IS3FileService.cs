namespace Application.Services.Interfaces;

public interface IS3FileService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken);
    Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken);
    Task DeleteAsync(string fileName, CancellationToken cancellationToken);
}