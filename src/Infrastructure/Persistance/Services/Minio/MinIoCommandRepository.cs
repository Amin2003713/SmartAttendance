using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Shifty.Application.Base.MinIo.Commands.UploadPdf;
using Shifty.Application.Base.MinIo.Commands.UplodeFile;
using Shifty.Application.Interfaces.HangFire;
using Shifty.Application.Interfaces.Minio;
using Shifty.Common.General.Enums.FileType;
using Shifty.Common.Utilities.EnumHelpers;
using Shifty.Common.Utilities.TypeConverters;
using Shifty.Domain.HubFiles;

namespace Shifty.Persistence.Services.Minio;

public class MinIoCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<HubFile>> logger,
    IHangFireJobRepository hangFireJobRepository
)
    : CommandRepository<HubFile>(dbContext, logger),
        IMinIoCommandRepository
{
    private readonly AmazonS3Client _s3Client = new(ApplicationConstant.Minio.AccessKey,
        ApplicationConstant.Minio.SecretKey,
        new AmazonS3Config
        {
            ServiceURL = ApplicationConstant.Minio.Endpoint, // MinIO URL
            ForcePathStyle = true
        });

    public async Task<HubFile> UploadFileAsync(UploadFileCommand file, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(file.Path))
                throw new ArgumentNullException(nameof(file.Path));

            if (file?.File == null)
                throw new ArgumentNullException(nameof(file));

            file.Path = Path.Combine(file.Path, $"file-{DateTime.UtcNow:hh:mm:ss}-{file.File.FileName}").ToLower();
            var (bucketName, objectName) = ParsePath(file.Path);
            await EnsureBucketExistsAsync(bucketName);

            var fileType = file.File.GetFileType();

            await using var stream = await file.File.OpenReadStream().CompressIfImageAsync(fileType);
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = stream,
                ContentType = file.File.ContentType
            };

            await _s3Client.PutObjectAsync(request, cancellationToken);

            return new HubFile
            {
                Path = file.Path,

                Name = file.File.FileName,
                ReferenceIdType = file.RowType,
                ReportDate = file.ReportDate,
                ReferenceId = file.RowId,
                Type = fileType
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<HubFile> UploadPdfAsync(UploadPdfCommand file, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(file.Path))
            throw new ArgumentNullException(nameof(file.Path));

        if (file.File == null || file.File.Length == 0)
            throw new ArgumentNullException(nameof(file.File));

        file.Path = Path.Combine(file.Path, file.FileName);
        var (bucketName, objectName) = ParsePath(file.Path);
        await EnsureBucketExistsAsync(bucketName);

        await using var stream = new MemoryStream(file.File);
        await UploadObjectAsync(bucketName, objectName, stream, file.File.Length, "application/pdf", cancellationToken);

        var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
        var hubFile = new HubFile
        {
            Path = file.Path,

            Name = file.FileName,
            ReferenceIdType = file.RowType,
            ReportDate = DateTime.UtcNow,
            ReferenceId = file.RowId,
            Type = extension switch
                   {
                       "pdf" => FileType.Pdf, "zip" => FileType.Zip, _ => FileType.Picture
                   }
        };

        hangFireJobRepository.AddDelayedJob(
            () => DeletePdfFile(hubFile.Path),
            TimeSpan.FromMinutes(30));

        return hubFile;
    }

    public async Task<HubFile> UploadExcelAsync(UploadPdfCommand file, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(file.Path))
            throw new ArgumentNullException(nameof(file.Path));

        if (file.File == null || file.File.Length == 0)
            throw new ArgumentNullException(nameof(file.File));

        file.Path = Path.Combine(file.Path, file.FileName);
        var (bucketName, objectName) = ParsePath(file.Path);
        await EnsureBucketExistsAsync(bucketName);

        await using var stream = new MemoryStream(file.File);
        await UploadObjectAsync(bucketName,
            objectName,
            stream,
            file.File.Length,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            cancellationToken);

        var hubFile = new HubFile
        {
            Path = file.Path,

            Name = file.FileName,
            ReferenceIdType = file.RowType,
            ReportDate = DateTime.UtcNow,
            ReferenceId = file.RowId,
            Type = FileType.Excel
        };

        hangFireJobRepository.AddDelayedJob(() => DeletePdfFile(hubFile.Path), TimeSpan.FromMinutes(30));

        return hubFile;
    }

    public async Task<HubFile> UploadXmlAsync(UploadPdfCommand file, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(file.Path)) throw new ArgumentNullException(nameof(file.Path));
        if (file.File == null || file.File.Length == 0) throw new ArgumentNullException(nameof(file.File));

        file.Path = Path.Combine(file.Path, file.FileName);
        var (bucketName, objectName) = ParsePath(file.Path);
        await EnsureBucketExistsAsync(bucketName);

        await using var stream = new MemoryStream(file.File);
        await UploadObjectAsync(bucketName, objectName, stream, file.File.Length, "application/xml", cancellationToken);

        var hubFile = new HubFile
        {
            Path = file.Path,

            Name = file.FileName,
            ReferenceIdType = file.RowType,
            ReportDate = DateTime.UtcNow,
            ReferenceId = file.RowId,
            Type = FileType.Xml
        };

        hangFireJobRepository.AddDelayedJob(() => DeletePdfFile(hubFile.Path), TimeSpan.FromMinutes(30));

        return hubFile;
    }

    public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        try
        {
            var (bucketName, objectName) = ParsePath(filePath);
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = objectName
            };

            await _s3Client.DeleteObjectAsync(request, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($@"Error deleting file '{filePath}': {ex.Message}");
            return false;
        }
    }

    public void DeletePdfFile(string filePath)
    {
        DeleteFileAsync(filePath, CancellationToken.None).GetAwaiter().GetResult();
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
        var listResponse = await _s3Client.ListBucketsAsync();

        var buckets = listResponse.Buckets ?? new List<S3Bucket>(); // 🛡️ ضد Null

        if (!buckets.Any(b => b.BucketName == bucketName))
        {
            await _s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName
            });
        }
    }
    private async Task UploadObjectAsync(
        string bucketName,
        string objectName,
        Stream stream,
        long size,
        string contentType,
        CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
            InputStream = stream,
            ContentType = contentType,
            AutoCloseStream = false,
            Headers =
            {
                ContentLength = size
            }
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
    }
}