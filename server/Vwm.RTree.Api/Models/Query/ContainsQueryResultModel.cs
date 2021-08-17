namespace Vwm.RTree.Api.Models.Query
{
  public class ContainsQueryResultModel
  {
    public bool Contains { get; set; }
    public long TreeMilliseconds { get; set; }
    public Point RequestPoint { get; set; }
  }
}
