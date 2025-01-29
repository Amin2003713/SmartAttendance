using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shifty.ApiFramework.Swagger
{
    public class ApplyHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation , OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "--tenant--" , In = ParameterLocation.Header , Description = "your Organization header" , Required = false ,
            });


            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Device-Type" , In = ParameterLocation.Header , Description = "your Organization header" , Required = true ,
                Schema = new OpenApiSchema
                {
                    Type = "string" , Default = new OpenApiString("Browser") ,
                } ,
            });
        }
    }
}