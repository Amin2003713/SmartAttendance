using SmartAttendance.Application.Features.Documents.Requests;
using SmartAttendance.Application.Features.Documents.Responses;

namespace SmartAttendance.Application.Features.Documents.Commands;

// Command: ثبت متادیتای سند
public sealed record UploadDocumentCommand(
    UploadDocumentRequest Request
) : IRequest<DocumentDto>;

// Handler: ثبت متادیتا