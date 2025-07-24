using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shifty.ApiFramework.Swagger;

public class EnumModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
        if (!string.IsNullOrEmpty(value) && Enum.TryParse(bindingContext.ModelType, value, true, out var result))
            bindingContext.Result = ModelBindingResult.Success(result);
        else
            bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                $"Invalid value for {bindingContext.ModelType.Name}.");

        return Task.CompletedTask;
    }
}