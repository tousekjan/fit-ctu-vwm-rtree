using Microsoft.Extensions.DependencyInjection;

namespace Vwm.RTree.Api.IoC
{
  public static class Loggers
  {
    public static void AddLoggers(this IServiceCollection services)
    {
      services.AddTransient<TreeLogger>();
    }
  }
}
