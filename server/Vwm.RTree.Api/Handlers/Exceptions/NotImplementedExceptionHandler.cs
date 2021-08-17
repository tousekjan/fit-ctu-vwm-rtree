using System;
using Vwm.RTree.Api.Errors;

namespace Vwm.RTree.Api.Handers.Exceptions
{
  public class NotImplementedExceptionHandler: ExceptionHandlerBase<NotImplementedException>
  {
    public override ApiError Handle(Exception exception) => 
      new ApiError($"{exception.Message} : {exception.StackTrace}", HttpStatusCodes._NotImplemented);
  }
}
