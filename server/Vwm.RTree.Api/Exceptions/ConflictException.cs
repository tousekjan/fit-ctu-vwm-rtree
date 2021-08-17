namespace Vwm.RTree.Api.Exceptions
{
  public class ConflictException: ApiExceptionBase
  {
    public ConflictException(string message)
      : base(message, HttpStatusCodes._Conflict)
    {
    }
  }
}
