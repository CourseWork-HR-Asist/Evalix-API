using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Services.Interfaces;


namespace Application.Services.S3;

public class S3FileService : IS3FileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "evalix";

    public S3FileService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _bucketName,
            ContentType = contentType
        };

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(uploadRequest, cancellationToken);

        return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
    }

    public async Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, fileName, cancellationToken);
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0; 
            return memoryStream;
        }
        catch (AmazonS3Exception e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null; 
        }
    }

    public async Task DeleteAsync(string fileName, CancellationToken cancellationToken)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        await _s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
    }
}
