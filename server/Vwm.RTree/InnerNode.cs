using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree
{
  public class InnerNode : NodeBase
  {
    public HashSet<int> ChildrenIds { get; }

    public InnerNode(int id, Rectangle boundingRectangle, int[] childrenIds)
      : base(id, boundingRectangle)
    {
      ChildrenIds = new HashSet<int>(childrenIds);
    }

    public InnerNode(int id, params int[] childrenIds)
      : base(id)
    {
      ChildrenIds = new HashSet<int>(childrenIds);
    }

    public void Insert(NodeBase node)
    {
      ChildrenIds.Add(node.Id);

      if (BoundingRectangle == null)
      {
        var newCoords = new (double, double)[node.BoundingRectangle.Coords.Length];
        node.BoundingRectangle.Coords.CopyTo(newCoords, 0);
        BoundingRectangle = new Rectangle(newCoords);
      }
      else
        BoundingRectangle.Merge(node.BoundingRectangle);
    }

    public override SnapshotNode ToSnapshot()
    {
      return new SnapshotNode
      {
        ChildrenIds = ChildrenIds.ToArray(),
        Id = Id,
        BoundingRectangle = BoundingRectangle?.ToSnapshot()
      };
    }
  }
}
