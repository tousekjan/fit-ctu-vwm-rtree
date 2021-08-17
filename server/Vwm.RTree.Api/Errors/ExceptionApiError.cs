using System;
using Newtonsoft.Json;

namespace Vwm.RTree.Api.Errors
{
  public class ExceptionApiError: ApiError
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string StackTrace { get; }

    public ExceptionApiError(Exception ex)
      : base(ex.Message, HttpStatusCodes._InternalServerError)
    {
      StackTrace = ex.StackTrace;
    }
  }
}
