namespace Vwm.RTree.Api.Models.Query
{
  public class RangeQueryResultModel
  {
    public RangeQueryResultModel(Point requestPoint, double requestRange)
    {
      RequestPoint = requestPoint;
      RequestRange = requestRange;
    }

    public Point[] TreeResult { get; set; }
    public ulong? TreeMilliseconds { get; set; }
    public int TreeResultCount { get; set; }
    public bool TreeResultShrinked { get; set; }

    public Point RequestPoint { get; }
    public double RequestRange { get; }
  }
}
