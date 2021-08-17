using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Vwm.RTree.Api.Swagger;

namespace Vwm.RTree.Api.Configs
{
  public static class SwaggerConfig
  {
    public const string _SecurityDefinitionName = "Api key";
    public const string _V1Route = "v1";
    public const string _V1Title = "Vwm RTree";

    public static void SetupSwagger(SwaggerOptions options)
    {
      options.RouteTemplate = "docs/{documentName}/swagger.json";
    }

    public static void SetupSwaggerGen(SwaggerGenOptions options)
    {
      options.OperationFilter<ConsumesProducesAttributeOperationFilter>();
      options.OperationFilter<CancellationTokenOperationFilter>();

      options.SwaggerDoc(_V1Route, new Info { Title = _V1Title, Version = _V1Route });
      options.DocInclusionPredicate((version, apiDescription) => apiDescription.RelativePath.Contains($"/{version}/"));
      options.DescribeAllEnumsAsStrings();

      foreach (string xmlFile in Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.xml", SearchOption.AllDirectories))
        options.IncludeXmlComments(xmlFile);
    }

    public static void SetupSwaggerUI(SwaggerUIOptions options)
    {
      options.SwaggerEndpoint($"http://localhost:8080/docs/v1/swagger.json", _V1Route);
    }
  }
}
