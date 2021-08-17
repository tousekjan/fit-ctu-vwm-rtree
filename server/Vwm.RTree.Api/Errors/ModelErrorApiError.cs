using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Vwm.RTree.Api.Errors
{
  public class ModelErrorApiError: ApiError
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ModelError[] Errors { get; }

    public ModelErrorApiError(string message, int statusCode, ModelError[] errors)
      : base(message, statusCode)
    {
      Errors = errors;
    }
  }
}
