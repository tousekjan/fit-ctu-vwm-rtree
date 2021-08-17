namespace Vwm.RTree.Api.Exceptions
{
  public class ForbiddenException: ApiExceptionBase
  {
    public ForbiddenException(string message)
      : base(message, HttpStatusCodes._Forbidden)
    {
    }
  }
}
