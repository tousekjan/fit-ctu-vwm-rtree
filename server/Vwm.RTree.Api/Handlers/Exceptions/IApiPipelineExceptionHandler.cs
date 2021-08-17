using System;
using Vwm.RTree.Api.Errors;

namespace Vwm.RTree.Api.Handers.Exceptions
{
  public interface IApiPipelineExceptionHandler
  {
    bool CanHandle(Exception exception);
    ApiError Handle(Exception exception);
  }
}
