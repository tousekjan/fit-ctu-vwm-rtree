namespace Vwm.RTree.Api.Models.Query
{
  public class BenchmarkKNNQueryResultModel
  {
    public BenchmarkKNNQueryResultModel(Point requestPoint, int requestCount)
    {
      RequestPoint = requestPoint;
      RequestCount = requestCount;
    }

    public Point[] LinearResult { get; set; }
    public Point[] TreeResult { get; set; }

    public ulong? LinearMilliseconds { get; set; }
    public ulong? TreeMilliseconds { get; set; }


    public int LinearResultCount { get; set; }
    public int TreeResultCount { get; set; }

    public bool LinearResultShrinked { get; set; }
    public bool TreeResultShrinked { get; set; }

    public bool ResultsIdentical { get; set; }

    public Point RequestPoint { get; }
    public int RequestCount { get; }
  }
}
