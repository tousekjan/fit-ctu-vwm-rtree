using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vwm.RTree.Api.Swagger
{
  public class RouteControllerNameOperationFilter: IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      var routeAttribute = (RouteAttribute)context
        .MethodInfo
        .DeclaringType
        .GetCustomAttributes(false)
        .FirstOrDefault(x => x.GetType() == typeof(RouteAttribute));

      if (routeAttribute == null)
        return;

      operation.Tags = new[] { routeAttribute.Name };
    }
  }
}
