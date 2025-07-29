using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdempotentApi.Headers;

public class AddIdempotencyHeaderoperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        if (context.ApiDescription.HttpMethod == HttpMethod.Post.ToString())
        {

            OpenApiParameter header = new OpenApiParameter
            {
                Name = "Idempotency-Key",
                AllowEmptyValue = false,
                In = ParameterLocation.Header,
                Description = "Idempotency Key to ensure idempotent operations",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "uuid",
                }
            };

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(header);
        }
    }
}
