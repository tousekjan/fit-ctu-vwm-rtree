using System;
using Vwm.RTree.Api.Errors;

namespace Vwm.RTree.Api.Handers.Exceptions
{
  public abstract class ExceptionHandlerBase<TException>: IApiPipelineExceptionHandler
    where TException : Exception
  {
    public bool CanHandle(Exception exception) => exception is TException;
    public abstract ApiError Handle(Exception exception);
  }
}
