﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vwm.RTree.Api.Swagger
{
  public class CancellationTokenOperationFilter: IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      var apiDescription = context.ApiDescription;
      var excludedParameters = apiDescription.ParameterDescriptions
          .Where(p =>
              p.ModelMetadata?.ContainerType == typeof(CancellationToken) ||
              p.ModelMetadata?.ContainerType == typeof(WaitHandle) ||
              p.ModelMetadata?.ContainerType == typeof(SafeWaitHandle))
          .Select(p => operation.Parameters.FirstOrDefault(operationParam => operationParam.Name == p.Name));

      foreach (var parameter in excludedParameters)
        operation.Parameters.Remove(parameter);
    }
  }
}
