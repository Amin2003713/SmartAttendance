using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shifty.ApiFramework.Tools;
using Shifty.Persistence.Services.Identities;

namespace Shifty.ApiFramework.Attributes;

public class ProjectAccessAttribute(
    params int[] requiredAccess
)
    : ActionFilterAttribute
{
    private readonly string projectIdKey = "projectId";

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // todo : handle the auth 

        // var services  = context.HttpContext.RequestServices;
        // var localizer = services.GetRequiredService<IStringLocalizer<ProjectAccessAttribute>>();
        // // var broker    = services.GetRequiredService<IMessageBroker>();
        //
        // if (!AuthAttributeCache.HasAuthenticationAttributes(context))
        // {
        //     await next();
        //     return;
        // }
        //
        // var identityService = services.GetService<IdentityService>();
        // var mediator        = services.GetService<IMediator>();
        //
        // if (identityService == null || mediator == null)
        // {
        //     SetErrorResponse(context,
        //         StatusCodes.Status500InternalServerError,
        //         localizer["Server Error"],
        //         localizer["An unexpected error occurred."]);
        //
        //     return;
        // }
        //
        // var userId = identityService.GetUserId();
        //
        // if (userId == Guid.Empty)
        // {
        //     SetErrorResponse(context,
        //         StatusCodes.Status401Unauthorized,
        //         localizer["Unauthorized"],
        //         localizer["User is not authenticated."]);
        //
        //     return;
        // }
        //
        //
        //
        // if (projectId == null || projectId == Guid.Empty)
        // {
        //     await next();
        //     return;
        // }
        //
        // var userAccess = await broker
        //     .RequestAsync<GetProjectUserAccessBrokerResponse, GetProjectUserAccessBroker>
        //         (new GetProjectUserAccessBroker(projectId.Value, userId!.Value!));
        //
        // if (userAccess == null)
        // {
        //     SetErrorResponse(context,
        //         StatusCodes.Status403Forbidden,
        //         localizer["Forbidden"],
        //         localizer["User does not have the required access to this project."]);
        //
        //     return;
        // }
        //
        // if (!userAccess.IsGodMode)
        // {
        //     await next();
        //     return;
        // }
        //
        //
        // foreach (var access in requiredAccess)
        // {
        //     var method = context.HttpContext.Request.Method.ToUpperInvariant();
        //
        //     var permission = method switch
        //                      {
        //                          "GET"    => AccessPermission.Get,
        //                          "POST"   => AccessPermission.Post,
        //                          "PUT"    => AccessPermission.Put,
        //                          "DELETE" => AccessPermission.Delete,
        //                          _        => AccessPermission.None
        //                      };
        //
        //     if (userAccess.AccessList.HasAccess(access, permission))
        //         continue;
        //
        //     SetErrorResponse(context,
        //         StatusCodes.Status403Forbidden,
        //         localizer["Forbidden"],
        //         localizer[$"User is not allowed to perform {localizer[method].Value} on this resource."].Value);
        //
        //     return;
        // }

        await next();
    }

    private Guid? GetProjectIdFromRequest(HttpContext context, ActionExecutingContext actionContext)
    {
        if (context.Request.Query.TryGetValue(projectIdKey, out var queryValue) &&
            Guid.TryParse(queryValue, out var projectId) ||
            context.Request.HasFormContentType &&
            context.Request.Form.TryGetValue(projectIdKey, out var formValue) &&
            Guid.TryParse(formValue, out projectId))
            return projectId;


        if (context.Request.ContentType?.Contains("application/json") != true)
            return null;

        var bodyParam =
            actionContext.ActionDescriptor.Parameters.FirstOrDefault(p =>
                p.BindingInfo?.BindingSource == BindingSource.Body);

        if (bodyParam == null ||
            !actionContext.ActionArguments.TryGetValue(bodyParam.Name, out var body) ||
            body == null)
            return null;

        try
        {
            var jsonObject = JObject.Parse(JsonConvert.SerializeObject(body));

            return jsonObject.Properties()
                .FirstOrDefault(p => p.Name.Equals(projectIdKey, StringComparison.OrdinalIgnoreCase))
                ?.Value
                ?.ToObject<Guid>();
        }
        catch
        {
            return null;
        }
    }

    private static void SetErrorResponse(ActionExecutingContext context, int statusCode, string title, string detail)
    {
        context.Result = new ObjectResult(new ApiProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        })
        {
            StatusCode = statusCode
        };
    }
}