using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree
{
  public class LeafNode: NodeBase
  {
    public HashSet<Point> Points { get; }

    public bool IsFull => Points.Count >= Constants._MaxPointCount;

    public LeafNode(int id, Rectangle boundingRectangle, Point[] points)
      : base(id, boundingRectangle)
    {
      Points = new HashSet<Point>(points);
    }

    public LeafNode(int id)
      : base(id)
    {
      Points = new HashSet<Point>();
    }

    public LeafNode(int id, Point point)
      : base(id)
    {
      Points = new HashSet<Point>();
      Insert(point);
    }

    protected override void DoInsert(Point point)
    {
      Points.Add(point);
    }

    public (LeafNode, LeafNode) Split(int id, Point newPoint, ISplitManager splitManager)
    {
      return splitManager.SplitLeaf(Points, Id, id, newPoint);
    }

    public override SnapshotNode ToSnapshot()
    {
      return new SnapshotNode
      {
        BoundingRectangle = BoundingRectangle?.ToSnapshot(),
        Id = Id,
        Points = Points.Select(x => x.ToSnapshot()).ToArray()
      };
    }
  }
}
