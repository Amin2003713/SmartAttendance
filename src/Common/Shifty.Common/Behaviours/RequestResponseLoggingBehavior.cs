using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shifty.Common.Utilities.LoggerHelper;

namespace Shifty.Common.Behaviours;

public class RequestResponseLoggingBehavior<TRequest, TResponse>(
    ILogger<RequestResponseLoggingBehavior<TRequest, TResponse>> _logger,
    IHttpContextAccessor _httpContextAccessor
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private const int MaxStringLength = 500;

    private readonly static JsonSerializerOptions _jsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true
    };

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var correlationId = GetOrCreateCorrelationId();

        // return await next(cancellationToken);

        LogRequest(request, correlationId);

        var response = await next(cancellationToken);

        LogResponse(response, correlationId);

        return response;
    }

    private Guid GetOrCreateCorrelationId()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context?.Request.Headers.TryGetValue("X-Correlation-ID", out var header) == true &&
            Guid.TryParse(header, out var existing))
            return existing;

        return Guid.CreateVersion7(DateTimeOffset.UtcNow);
    }

    private void LogRequest(TRequest request, Guid correlationId)
    {
        var name        = typeof(TRequest).Name;
        var safeRequest = SafeLoggingHelper.CreateSafeLogObject(request, _logger);
        var payload     = JsonSerializer.Serialize(safeRequest, _jsonOptions);

        _logger.LogInformation(
            "➡️ {RequestName} started. CorrelationId={CorrelationId} \n Payload={Payload}",
            name,
            correlationId,
            payload);
    }

    private void LogResponse(TResponse response, Guid correlationId)
    {
        var name         = typeof(TRequest).Name;
        var safeResponse = SafeLoggingHelper.CreateSafeLogObject(response, _logger);
        var payload      = JsonSerializer.Serialize(safeResponse, _jsonOptions);

        _logger.LogInformation(
            "✅ {RequestName} completed. CorrelationId={CorrelationId} Payload={Payload}",
            name,
            correlationId,
            payload);
    }
}