using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Shifty.ApiFramework.Attributes;

public static class AuthAttributeCache
{
    private readonly static ConcurrentDictionary<MethodInfo, bool> _authCache = new();

    public static bool HasAuthenticationAttributes(ActionContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            return false;

        var methodInfo = actionDescriptor.MethodInfo;

        // Check if the result is already cached
        if (_authCache.TryGetValue(methodInfo, out var cachedResult)) return cachedResult;

        // Check for authentication attributes on method or controller
        var hasAuthAttribute = methodInfo.GetCustomAttributes<AuthorizeAttribute>().Any() ||
                               methodInfo.GetCustomAttributes<ProjectAccessAttribute>().Any() ||
                               methodInfo.DeclaringType?.GetCustomAttributes<AuthorizeAttribute>().Any() ==
                               true ||
                               methodInfo.DeclaringType?.GetCustomAttributes<ProjectAccessAttribute>().Any() ==
                               true;

        // Cache the result for future use
        _authCache[methodInfo] = hasAuthAttribute;

        return hasAuthAttribute;
    }
}