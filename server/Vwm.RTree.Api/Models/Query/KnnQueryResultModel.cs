namespace Vwm.RTree.Api.Models.Query
{
  public class KNNQueryResultModel
  {
    public KNNQueryResultModel(Point requestPoint, int requestCount)
    {
      RequestPoint = requestPoint;
      RequestCount = requestCount;
    }

    public Point[] TreeResult { get; set; }

    public ulong? TreeMilliseconds { get; set; }

    public int TreeResultCount { get; set; }

    public bool TreeResultShrinked { get; set; }

    public Point RequestPoint { get; }
    public int RequestCount { get; }
  }
}
