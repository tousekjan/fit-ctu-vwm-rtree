using System.Linq;

namespace Vwm.RTree
{
  public class Snapshot
  {
    public int NextNodeId { get; set; }

    public int RootNodeId { get; set; }

    public int Dimensions { get; set; }

    public int MaxNumberOfChildren { get; set; }

    public int MinNumberOfChildren { get; set; }

    public int MaxPointCount { get; set; }

    public int CacheSize { get; set; }

    public SplitType SplitType { get; set; } 

    public SnapshotNode[] Nodes { get; set; }
  }

  public class SnapshotPoint
  {
    public double[] Coords { get; set; }

    public Point ToPoint()
    {
      return new Point(Coords);
    }
  }

  public class SnapshotNode
  {
    public int[] ChildrenIds { get; set; }

    public SnapshotPoint[] Points { get; set; }

    public SnapshotRectangle BoundingRectangle { get; set; }

    public int Id { get; set; }

    public NodeBase ToNode()
    {
      if(ChildrenIds == null)
        return new LeafNode(Id, BoundingRectangle?.ToRectangle(), Points.Select(x => x.ToPoint()).ToArray());

      return new InnerNode(Id, BoundingRectangle?.ToRectangle(), ChildrenIds);
    }
  }

  public class SnapshotRectangle
  {
    public (double Minimum, double Maximum)[] Coords { get; set; }

    public Rectangle ToRectangle()
    {
      return new Rectangle(Coords);
    }
  }
}
