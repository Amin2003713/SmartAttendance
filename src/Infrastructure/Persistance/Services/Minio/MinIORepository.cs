using Shifty.Common;
using Shifty.Domain.Interfaces.Minio;

namespace Shifty.Persistence.Services.Minio
{
    public class MinIORepository : IMinIoRepository , IScopedDependency
    {
        // private readonly MinioClient _client = (MinioClient)new MinioClient()
        //                                                     .WithEndpoint(ApplicationConstant.MinioEndpoint)
        //                                                     .WithCredentials(ApplicationConstant.MinioAccessKey , ApplicationConstant.MinioSecretKey)
        //                                                     .Build();
        //
        // public async Task DeleteFileAsync(string bucketName, string objectName, CancellationToken cancellationToken)
        // {
        //     try
        //     {
        //         await _client.RemoveObjectAsync(bucketName, objectName, cancellationToken);
        //         Console.WriteLine($"File {objectName} deleted successfully.");
        //     }
        //     catch (S3Exception ex)
        //     {
        //         Console.WriteLine($"Error deleting file: {ex.Message}");
        //         // Handle exception or log error
        //     }
        // }
        //
        // public async Task GetFileAsync(string bucketName, string objectName, string filePath, CancellationToken cancellationToken)
        // {
        //     try
        //     {
        //         await _client.GetObjectAsync(bucketName, objectName, filePath, cancellationToken);
        //         Console.WriteLine($"File {objectName} downloaded to {filePath}.");
        //     }
        //     catch (S3Exception ex)
        //     {
        //         Console.WriteLine($"Error downloading file: {ex.Message}");
        //         // Handle exception or log error
        //     }
        // }
        //
        // public async Task GetFilesAsync(string bucketName, string[] objectNames, string downloadPath, CancellationToken cancellationToken)
        // {
        //     var tasks = new List<Task>();
        //     foreach (var obj in objectNames)
        //     {
        //         string filePath = Path.Combine(downloadPath, obj);
        //         tasks.Add(GetFileAsync(bucketName, obj, filePath, cancellationToken));
        //     }
        //     await Task.WhenAll(tasks);
        // }
        //
        // public async Task UploadFileAsync(string bucketName, string objectName, string filePath, CancellationToken cancellationToken)
        // {
        //     if (!File.Exists(filePath))
        //     {
        //         throw new FileNotFoundException($"File not found: {filePath}");
        //     }
        //
        //     using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //     {
        //         try
        //         {
        //             await _client.PutObjectAsync(bucketName, objectName, stream, Path.GetExtension(filePath), cancellationToken: cancellationToken);
        //             Console.WriteLine($"File {objectName} uploaded successfully.");
        //         }
        //         catch (S3Exception ex)
        //         {
        //             Console.WriteLine($"Error uploading file: {ex.Message}");
        //             // Handle exception or log error
        //         }
        //     }
        // }
        //
        // public async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken)
        // {
        //     try
        //     {
        //         if (!await _client.BucketExistsAsync(bucketName, cancellationToken))
        //         {
        //             await _client.MakeBucketAsync(bucketName, cancellationToken);
        //             Console.WriteLine($"Bucket {bucketName} created successfully.");
        //         }
        //     }
        //     catch (S3Exception ex)
        //     {
        //         Console.WriteLine($"Error creating bucket: {ex.Message}");
        //         // Handle exception or log error
        //     }
        // }
    }
}