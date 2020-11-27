using EncompassWrapperConstants;
using Swagger.Net;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace EncompassWrapperAPI
{
    public class HeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            if (!apiDescription.RelativePath.Contains("api/Token/"))
            {
                operation.parameters.Add(new Parameter
                {
                    name = HeaderConstant.TokenHeader,
                    @in = "header",
                    description = "Access Token",
                    type = "string",
                    required = true
                });

                operation.parameters.Add(new Parameter
                {
                    name = HeaderConstant.TokenTypeHeader,
                    @in = "header",
                    description = "Access Token Type",
                    type = "string",
                    required = true
                });
            }
        }
    }
}
