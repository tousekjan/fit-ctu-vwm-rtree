namespace Vwm.RTree.Api.Models.Query
{
  public class NnQueryResultModel
  {
    public NnQueryResultModel(Point requestPoint)
    {
      RequestPoint = requestPoint;
    }

    public Point TreeResult { get; set; }

    public ulong? TreeMilliseconds { get; set; }

    public Point RequestPoint { get; }
  }
}
