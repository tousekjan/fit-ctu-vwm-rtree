using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vwm.RTree.Api.Configs;
using Vwm.RTree.Api.IoC;
using Vwm.RTree.Api.Middleware;
using Vwm.RTree.Linear;

namespace Vwm.RTree.Api
{
  public class Startup
  {
    private readonly DataGenConfig fDataGenConfig;

    public Startup(IConfiguration configuration)
    {
      fDataGenConfig = configuration
        .GetSection("DataGen")
        ?.Get<DataGenConfig>();

      configuration
        .GetSection("Constants")
        ?.Get<ConstantsConfig>()
        ?.Apply();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddLogging();
      services.AddCors();
      services.AddOptions();

      services.AddMvc(MvcConfig.SetupMvcOptions);
      services.AddSwaggerGen(SwaggerConfig.SetupSwaggerGen);

      services.AddFactories();
      services.AddHandlers();
      services.AddLoggers();

      var logger = services
        .BuildServiceProvider()
        .GetService<TreeLogger>();

      var tree = new Tree(logger);
      var pointHolder = new PointHolder();

      if (fDataGenConfig != null)
        DataGenProccessor.Fill(fDataGenConfig, tree, pointHolder);

      services.AddSingleton(tree);
      services.AddSingleton(pointHolder);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseCors(CorsConfig.Setup);
      app.UseApiExceptionHandling();
      app.UseMvc();
      app.UseSwagger(SwaggerConfig.SetupSwagger);
      app.UseSwaggerUI(SwaggerConfig.SetupSwaggerUI);
    }
  }
}
