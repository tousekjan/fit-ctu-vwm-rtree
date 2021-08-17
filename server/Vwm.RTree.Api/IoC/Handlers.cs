using Microsoft.Extensions.DependencyInjection;
using Vwm.RTree.Api.Handers.Exceptions;

namespace Vwm.RTree.Api.IoC
{
  public static class Handlers
  {
    public static void AddHandlers(this IServiceCollection services)
    {
      services.AddAllTypesOf<IApiPipelineExceptionHandler>();
    }
  }
}
