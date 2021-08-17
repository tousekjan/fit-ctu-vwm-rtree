namespace Vwm.RTree.Api.Exceptions
{
  public class NotFoundException: ApiExceptionBase
  {
    public NotFoundException(string message) 
      : base(message, HttpStatusCodes._NotFound)
    {
    }
  }
}
