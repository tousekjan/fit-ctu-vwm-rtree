using System;

namespace Vwm.RTree.Api.Exceptions
{
  public abstract class ApiExceptionBase: Exception
  {
    public int StatusCode { get; }

    public ApiExceptionBase(string message, int statusCode)
      : base(message)
    {
      StatusCode = statusCode;
    }
  }
}
