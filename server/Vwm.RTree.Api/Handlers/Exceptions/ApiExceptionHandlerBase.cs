using System;
using Vwm.RTree.Api.Errors;
using Vwm.RTree.Api.Exceptions;

namespace Vwm.RTree.Api.Handers.Exceptions
{
  public abstract class ApiExceptionHandlerBase<TException>: ExceptionHandlerBase<TException>
    where TException : ApiExceptionBase
  {
    public override ApiError Handle(Exception exception)
    {
      var apiException = (ApiExceptionBase)exception;

      return new ApiError(apiException.Message, apiException.StatusCode);
    }
  }
}
