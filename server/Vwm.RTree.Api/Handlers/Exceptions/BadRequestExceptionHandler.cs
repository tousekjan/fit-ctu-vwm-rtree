using System;
using Vwm.RTree.Api.Errors;
using Vwm.RTree.Api.Exceptions;

namespace Vwm.RTree.Api.Handers.Exceptions
{
  public class BadRequestExceptionHandler: ApiExceptionHandlerBase<BadRequestException>
  {
    public override ApiError Handle(Exception exception)
    {
      var brException = (BadRequestException)exception;

      return new ModelErrorApiError(brException.Message, brException.StatusCode, brException.ModelErrors);
    }
  }
}
