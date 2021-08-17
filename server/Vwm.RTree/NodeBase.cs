using System;

namespace Vwm.RTree
{
  public abstract class NodeBase : IEquatable<NodeBase>
  {
    public int Id { get; }
    public Rectangle BoundingRectangle { get; set; }

    protected NodeBase(int id, Rectangle boundingRectangle)
    {
      Id = id;
      BoundingRectangle = boundingRectangle;
    }

    protected NodeBase(int id)
    {
      Id = id;
    }

    public double DifferenceWithPoint(Point point)
    {
      return BoundingRectangle?.DifferenceWithPoint(point) ?? 0;
    }

    public void Insert(params Point[] points)
    {
      foreach (var point in points)
      {
        UpdateBoundingRectangle(point);
        DoInsert(point);
      }
    }

    public void AddToBounding(Rectangle rectangle)
    {
      BoundingRectangle.Merge(rectangle);
    }

    protected void UpdateBoundingRectangle(Point point)
    {
      if (BoundingRectangle == null)
        BoundingRectangle = new Rectangle(point);
      else
        BoundingRectangle.AddPoint(point);
    }

    protected virtual void DoInsert(Point point) { }

    public bool Equals(NodeBase other)
    {
      return other.Id == Id;
    }

    public override int GetHashCode() => 2108858624 + Id.GetHashCode();

    public abstract SnapshotNode ToSnapshot();
  }
}
