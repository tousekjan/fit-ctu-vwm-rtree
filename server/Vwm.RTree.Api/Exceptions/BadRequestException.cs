using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Vwm.RTree.Api.Exceptions
{
  public class BadRequestException: ApiExceptionBase
  {
    public ModelError[] ModelErrors { get; set; }

    public BadRequestException(string message)
      : base(message, HttpStatusCodes._BadRequest) { }

    public BadRequestException(string message, ModelStateDictionary modelState)
      : base(message, HttpStatusCodes._BadRequest)
    {
      ModelErrors = GetModelStateErrors(modelState);
    }

    private ModelError[] GetModelStateErrors(ModelStateDictionary modelState)
    {
      if (modelState == null || modelState.ErrorCount < 1)
        return Array.Empty<ModelError>();

      return modelState
        .Where(model => model.Value.ValidationState == ModelValidationState.Invalid)
        .Select
        (
          model => new ModelError()
          {
            PropertyName = model.Key,
            Messages = model
              .Value
              .Errors
              .Select
              (
                error => string.IsNullOrEmpty(error.ErrorMessage)
                         ? error.Exception?.Message
                         : error.ErrorMessage
              )
              .ToArray()
          }
        )
        .ToArray();
    }
  }
}
