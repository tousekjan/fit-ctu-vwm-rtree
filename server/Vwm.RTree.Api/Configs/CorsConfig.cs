using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Vwm.RTree.Api.Configs
{
  public static class CorsConfig
  {
    public static void Setup(CorsPolicyBuilder _)
    {
      _.AllowAnyHeader();
      _.AllowAnyMethod();
      _.AllowAnyOrigin();
    }
  }
}
