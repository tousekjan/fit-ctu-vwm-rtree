using Microsoft.AspNetCore.Mvc.Filters;
using Vwm.RTree.Api.Exceptions;

namespace Vwm.RTree.Api.Attributes
{
  public class ApiValidationAttribute: ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
        throw new BadRequestException("Model is not valid.", context.ModelState);

      base.OnActionExecuting(context);
    }
  }
}
