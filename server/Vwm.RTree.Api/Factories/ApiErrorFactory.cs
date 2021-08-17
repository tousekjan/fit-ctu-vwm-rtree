using System;
using System.Collections.Generic;
using System.Linq;
using Vwm.RTree.Api.Errors;
using Vwm.RTree.Api.Handers.Exceptions;

namespace Vwm.RTree.Api.Factories
{
  public class ApiErrorFactory
  {
    private readonly IEnumerable<IApiPipelineExceptionHandler> fApiPipelineExceptionHandlers;

    public ApiErrorFactory(IEnumerable<IApiPipelineExceptionHandler> apiPipelineExceptionHandlers)
    {
      fApiPipelineExceptionHandlers = apiPipelineExceptionHandlers;
    }

    public ApiError Create(Exception exception)
    {
      var handler = fApiPipelineExceptionHandlers.SingleOrDefault(x => x.CanHandle(exception));

      if (handler != null)
        return handler.Handle(exception);

      return new ExceptionApiError(exception);
    }
  }
}
