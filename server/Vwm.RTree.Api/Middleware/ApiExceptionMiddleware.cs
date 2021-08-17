using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Vwm.RTree.Api.Factories;

namespace Vwm.RTree.Api.Middleware
{
  public class ApiExceptionMiddleware
  {
    private readonly RequestDelegate fNext;
    private readonly ApiErrorFactory fApiErrorFactory;

    public ApiExceptionMiddleware(RequestDelegate next, ApiErrorFactory apiErrorFactory)
    {
      fNext = next;
      fApiErrorFactory = apiErrorFactory;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await fNext(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var apiError = fApiErrorFactory.Create(exception);

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = apiError.StatusCode;
      return context.Response.WriteAsync(JsonConvert.SerializeObject(apiError), context.RequestAborted);
    }
  }

  public static class ApiExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
    {
      return app.UseMiddleware<ApiExceptionMiddleware>();
    }
  }
}
