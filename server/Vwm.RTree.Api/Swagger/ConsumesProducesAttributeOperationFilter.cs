using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vwm.RTree.Api.Swagger
{
  public class ConsumesProducesAttributeOperationFilter: IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      operation.Consumes = new[] { "application/json" };
      operation.Produces = new[] { "application/json" };
    }
  }
}
