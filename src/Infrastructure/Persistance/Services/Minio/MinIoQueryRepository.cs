using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Domain.HubFiles;

namespace SmartAttendance.Persistence.Services.Minio;

public class MinIoQueryRepository(
    ReadOnlyDbContext                 dbContext,
    ILogger<QueryRepository<HubFile>> logger
)
    : QueryRepository<HubFile>(dbContext, logger),
        IMinIoQueryRepository
{
    private AmazonS3Client? _s3Client;

    private AmazonS3Client S3Client => _s3Client ??= new AmazonS3Client(
        ApplicationConstant.Minio.AccessKey,
        ApplicationConstant.Minio.SecretKey,
        new AmazonS3Config
        {
            ServiceURL     = ApplicationConstant.Minio.Endpoint,
            ForcePathStyle = true,
            UseHttp        = true
            // RegionEndpoint = Amazon.RegionEndpoint.USEast1
        });

    public async Task<Stream> GetFileAsync(string file, CancellationToken cancellationToken)
    {
        var (bucketName, objectName) = ParsePath(file);

        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key        = objectName
        };

        using var response       = await S3Client.GetObjectAsync(request, cancellationToken);
        var       responseStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(responseStream, cancellationToken);
        responseStream.Position = 0;
        return responseStream;
    }

    private(string bucketName, string objectName) ParsePath(string path)
    {
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length < 2)
            throw new ArgumentException("The path must include a bucket name and an object name.");

        return (segments[0], string.Join("/", segments.Skip(1)));
    }

    private async Task EnsureBucketExistsAsync(string bucketName)
    {
        var listResponse = await S3Client.ListBucketsAsync();

        if (!listResponse.Buckets.Any(b => b.BucketName == bucketName))
            await S3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName
            });
    }
}