// using System.IO;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Mapster;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
// using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
// using SmartAttendance.Common.Common.Requests;
// using SmartAttendance.Common.Exceptions;
// using SmartAttendance.Common.General;
// using SmartAttendance.Common.General.Enums.FileType;
// using SmartAttendance.Common.General.Enums.Workflows;
// using SmartAttendance.Common.Utilities.InjectionHelpers;
// using SmartAttendance.Common.Utilities.TypeConverters;
//
// namespace SmartAttendance.Application.Base.HubFiles.Request.Commands.FileHelper;
//
// public class FileUploadHelper(
//     ILogger<FileUploadHelper> logger,
//     IMediator mediator
// ) : IScopedDependency
// {
//     public async Task<List<MediaFileStorage>?> ReplaceFilesAsync(
//         ItemTypes item,
//         List<UploadMediaFileRequest>? files,
//         List<MediaFileStorage>? oldFiles,
//         Guid projectId,
//         Guid aggregateId,
//         DateTime reported,
//         CancellationToken cancellationToken)
//     {
//         if (files == null || files.Count == 0)
//             return new List<MediaFileStorage>();
//
//         oldFiles ??= new List<MediaFileStorage>();
//         var newFileUrls = new List<MediaFileStorage>();
//         var uploadTasks = new List<Task<MediaFileStorage>>();
//
//         // Build a HashSet of new file URLs (from the incoming request)
//         var incomingUrls = files
//             .Where(f => f.MediaUrl != null)
//             .Select(f => f.MediaUrl!)
//             .ToHashSet();
//
//         // Delete any old file that’s no longer referenced
//         var deleteTasks = (
//                 from oldFile in oldFiles
//                 where !incomingUrls.Contains(oldFile.Url)
//                 select DeleteFileIfExists(oldFile.Url, aggregateId, cancellationToken))
//             .ToList();
//
//         // Process uploads and preserved files
//         foreach (var file in files)
//         {
//             var url = file.MediaUrl;
//             var stream = file.MediaFile;
//
//             // Case 1: Keep the old file (no new file, same URL)
//             if (url is not null && stream is null)
//             {
//                 // Ensure this URL actually exists in old files
//                 var existing = oldFiles.FirstOrDefault(f => f.Url == url);
//                 if (existing is not null)
//                     newFileUrls.Add(existing);
//             }
//             // Case 2: Replace old file with new content
//             else if (url is not null && stream is not null)
//             {
//                 deleteTasks.Add(DeleteFileIfExists(url, aggregateId, cancellationToken));
//                 uploadTasks.Add(UploadFile(item, stream, aggregateId, reported, projectId, cancellationToken));
//             }
//             // Case 3: Submitted file without existing URL
//             else if (url is null && stream is not null)
//             {
//                 uploadTasks.Add(UploadFile(item, stream, aggregateId, reported, projectId, cancellationToken));
//             }
//         }
//
//         await Task.WhenAll(deleteTasks);
//         var uploadedFiles = await Task.WhenAll(uploadTasks);
//         newFileUrls.AddRange(uploadedFiles);
//
//         return newFileUrls;
//     }
//
//     private async Task<MediaFileStorage> UploadFile(
//         ItemTypes item,
//         IFormFile file,
//         Guid aggregateId,
//         DateTime reported,
//         CancellationToken cancellationToken)
//     {
//         try
//         {
//             var fileStream = await file.ToByte(cancellationToken);
//
//             file = ToFormFile(FileContent, message.File);
//
//             var uploadCommand = new UploadHubFileCommand
//             {
//                 File = file,
//                 ReportDate = reported,
//                 RowType = (FileStorageType)item,
//                 RowId = aggregateId
//             };
//
//             var resultUrl = await mediator.Send(uploadCommand, cancellationToken);
//
//
//             return resultUrl.Adapt<MediaFileStorage>() ??
//                    throw SmartAttendanceException.InternalServerError("Problem uploading file(s)");
//         }
//         catch (Exception e)
//         {
//             logger.LogError(e, "Error occurred while uploading file for aggregate {AggregateId}", aggregateId);
//             throw;
//         }
//     }
//
//     private async Task DeleteFileIfExists(string fileUrl, Guid aggregateId, CancellationToken cancellationToken)
//     {
//         try
//         {
//             var result1 = await mediator.Send(new DeleteFileCommand(fileUrl),
//                 cancellationToken);
//
//
//             
//             if (!result1)
//             {
//                 logger.LogWarning("Failed to delete old file for aggregate {AggregateId}.", aggregateId);
//                 throw SmartAttendanceException.InternalServerError("Failed to delete file.");
//             }
//
//             logger.LogInformation("Deleted old file for aggregate {AggregateId}.", aggregateId);
//         }
//         catch (Exception e)
//         {
//             logger.LogError(e, "Error deleting file for aggregate {AggregateId}.", aggregateId);
//             throw;
//         }
//     }
//
//
//     private static IFormFile ToFormFile(byte[] bytes, string fileName, string contentType = "application/octet-stream")
//     {
//         var stream = new MemoryStream(bytes);
//
//         return new FormFile(stream, 0, bytes.Length, fileName, fileName)
//         {
//             Headers = new HeaderDictionary(),
//             ContentType = contentType
//         };
//     }
// }

