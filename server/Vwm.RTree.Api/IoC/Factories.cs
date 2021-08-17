using Microsoft.Extensions.DependencyInjection;
using Vwm.RTree.Api.Factories;

namespace Vwm.RTree.Api.IoC
{
  public static class Factories
  {
    public static void AddFactories(this IServiceCollection services)
    {
      services.AddTransient<ApiErrorFactory>();
    }
  }
}
