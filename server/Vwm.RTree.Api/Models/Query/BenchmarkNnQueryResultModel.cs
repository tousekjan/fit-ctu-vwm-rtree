namespace Vwm.RTree.Api.Models.Query
{
  public class BenchmarkNnQueryResultModel
  {
    public BenchmarkNnQueryResultModel(Point requestPoint)
    {
      RequestPoint = requestPoint;
    }

    public Point LinearResult { get; set; }
    public Point TreeResult { get; set; }

    public ulong? LinearMilliseconds { get; set; }
    public ulong? TreeMilliseconds { get; set; }

    public bool ResultsIdentical { get; set; }

    public Point RequestPoint { get; }
  }
}
