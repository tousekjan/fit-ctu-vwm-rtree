namespace Vwm.RTree.Api.Exceptions
{
  public class UnauthorizedException: ApiExceptionBase
  {
    public UnauthorizedException(string message)
      : base(message, HttpStatusCodes._Unauthorized)
    {
    }
  }
}
