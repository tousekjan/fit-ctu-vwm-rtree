using Microsoft.AspNetCore.Mvc;
using Vwm.RTree.Api.Attributes;

namespace Vwm.RTree.Api.Configs
{
  public class MvcConfig
  {
    public static void SetupMvcOptions(MvcOptions options)
    {
      options.Filters.Add<ApiValidationAttribute>();
    }
  }
}
