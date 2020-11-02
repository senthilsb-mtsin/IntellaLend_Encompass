using EncompassWrapperConstants;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace EncompassWrapperAPI
{
    public class HeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (!context.ApiDescription.RelativePath.Contains("EncompassToken"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeaderConstant.TokenHeader,
                    In = ParameterLocation.Header,
                    Required = true
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeaderConstant.TokenTypeHeader,
                    In = ParameterLocation.Header,
                    Required = true
                });
            }
        }
    }
}
