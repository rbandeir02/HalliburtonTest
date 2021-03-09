using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace HalliburtonTest
{
    public class AddCommonParameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && descriptor.ControllerName.StartsWith("Employee"))
            {
                //if (descriptor.ActionName == "Boarding")
                //{
                //    operation.Parameters.Add(new OpenApiParameter()
                //    {
                //        Name = "EmployeeId",
                //        In = ParameterLocation.Query,
                //        Description = "Id number of the employee.",
                //        Required = true
                //    });

                //    operation.Parameters.Add(new OpenApiParameter()
                //    {
                //        Name = "TripDate",
                //        In = ParameterLocation.Query,
                //        Description = "Boarding day in dd/MM/yyyy hh:mm:ss format.",
                //        Required = true
                //    });
                //}

            }
        }
    }
}