using System;
using System.Threading.Tasks;
using Vwm.RTree.Api.Errors;

namespace Vwm.RTree.Api.Handers.Exceptions
{
  public class TaskCanceledExceptionHandler: ExceptionHandlerBase<TaskCanceledException>
  {
    public override ApiError Handle(Exception exception) => 
      new ApiError($"Task was canceled.", HttpStatusCodes._TaskCanceled);
  }
}
