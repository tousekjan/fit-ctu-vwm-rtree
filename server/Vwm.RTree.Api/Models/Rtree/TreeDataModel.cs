using System.Collections.Generic;

namespace Vwm.RTree.Api.Models.RTree
{
  public class TreeDataModel
  {
    public bool Shrinked { get; set; }
    public int NodeCount { get; set; }
    public int RootNodeId { get; set; }
    public IEnumerable<NodeBase> Structure { get; set; }
  }
}
