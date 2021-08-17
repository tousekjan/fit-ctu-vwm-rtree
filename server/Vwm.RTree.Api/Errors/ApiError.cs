using Newtonsoft.Json;

namespace Vwm.RTree.Api.Errors
{
  public class ApiError
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; }

    public int StatusCode { get; }

    public ApiError(string message, int statusCode)
    {
      Message = message;
      StatusCode = statusCode;
    }
  }
}