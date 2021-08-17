using System;
using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree
{
  public class QuadraticSplitManager : ISplitManager
  {
    public void SplitInner(
      Stack<InnerNode> nodeChain,
      InnerNode node,
      NodeBase insertedNode1,
      NodeBase insertedNode2,
      NodeCache nodeCache,
      ref int rootNodeId,
      ref int nextNodeId)
    {
      if (node.ChildrenIds.Count < Constants._MaxNumberOfChildren)
      {
        node.Insert(insertedNode1);
        node.Insert(insertedNode2);
        EnlargeBoundingRectangles(nodeChain, node);

        return;
      }

      var nodes = new HashSet<NodeBase> { insertedNode2 };

      foreach (int id in node.ChildrenIds)
      {
        nodes.Add(nodeCache.GetById(id));
      }

      var (farthestNode1, farthestNode2, leftovers) = GetPivots(nodes);

      var newNode1 = new InnerNode(node.Id);
      var newNode2 = new InnerNode(nextNodeId++);

      newNode1.Insert(farthestNode1);
      newNode2.Insert(farthestNode2);

      while (leftovers.Any())
      {
        double? maxDiff = null;
        NodeBase maxNode = null;
        bool toFirst = false;

        foreach (var leftNode in leftovers)
        {
          var diff1 = newNode1.BoundingRectangle.DifferenceWithRectangle(leftNode.BoundingRectangle);
          var diff2 = newNode2.BoundingRectangle.DifferenceWithRectangle(leftNode.BoundingRectangle);
          var diff = Math.Abs(diff1 - diff2);

          if (maxDiff == null || diff > maxDiff.Value)
          {
            maxDiff = diff;
            maxNode = leftNode;
            if (diff2 > diff1)
              toFirst = true;
            else if (diff1 > diff2)
              toFirst = false;
            else if (newNode1.BoundingRectangle.Area < newNode2.BoundingRectangle.Area)
              toFirst = true;
            else
              toFirst = false;
          }
        }

        nodeCache.Add(maxNode);

        if (toFirst)
          newNode1.Insert(maxNode);
        else
          newNode2.Insert(maxNode);

        leftovers.Remove(maxNode);

        if (newNode1.ChildrenIds.Count == Constants._MinNumberOfChildren - leftovers.Count)
        {
          foreach (var nodeBase in leftovers)
          {
            nodeCache.Add(nodeBase);
            newNode1.Insert(nodeBase);
          }
          break;
        }
        else if (newNode2.ChildrenIds.Count == Constants._MinNumberOfChildren - leftovers.Count)
        {
          foreach (var nodeBase in leftovers)
          {
            nodeCache.Add(nodeBase);
            newNode2.Insert(nodeBase);
          }
          break;
        }
      }

      nodeCache[newNode2.Id] = newNode2;
      nodeCache[newNode1.Id] = newNode1;

      if (nodeChain.Any())
      {
        var parent = nodeChain.Pop();
#warning mozna spatne, vyresit ASAP
        parent.Insert(newNode1);
        SplitInner(nodeChain, parent, newNode1, newNode2, nodeCache, ref rootNodeId, ref nextNodeId);
      }
      else
      {
        var newRoot = new InnerNode(nextNodeId++);
        rootNodeId = newRoot.Id;
        newRoot.Insert(newNode1);
        newRoot.Insert(newNode2);

        nodeCache.Add(newRoot);
      }
    }

    private static void EnlargeBoundingRectangles(Stack<InnerNode> nodeChain, NodeBase node)
    {
      while (nodeChain.Any())
      {
        var currentNode = nodeChain.Pop();

        currentNode.BoundingRectangle.Merge(node.BoundingRectangle);
      }
    }

    private static (NodeBase, NodeBase, HashSet<NodeBase>) GetPivots(HashSet<NodeBase> leftovers)
    {
      (NodeBase p1, NodeBase p2) = (null, null);

      var nodes = leftovers.ToList();

      double? maxDiff = null;

      for (int i = 0; i < nodes.Count; i++)
      {
        for (int j = i + 1; j < nodes.Count; j++)
        {
          var diff = nodes[i].BoundingRectangle.Copy().DifferenceWithRectangle(nodes[j].BoundingRectangle)
            - nodes[i].BoundingRectangle.Area - nodes[j].BoundingRectangle.Area;

          if (!maxDiff.HasValue || diff > maxDiff.Value)
          {
            p1 = nodes[i];
            p2 = nodes[j];
            maxDiff = diff;
          }
        }
      }

      leftovers.Remove(p1);
      leftovers.Remove(p2);

      return (p1, p2, leftovers);
    }


    public (LeafNode, LeafNode) SplitLeaf(HashSet<Point> points, int id1, int id2, Point newPoint)
    {
      points.Add(newPoint);
      var (point1, point2, leftovers) = FindPivots(points);
      var leaf1 = new LeafNode(id1, point1);
      var leaf2 = new LeafNode(id2, point2);

      int difference = 0;
      while (true)
      {
        if (leftovers.Count < 1)
          break;

        double? maxDiff = null;
        Point maxPoint = null;
        bool toFirst = false;
        foreach (var point in leftovers)
        {
          var diff1 = leaf1.BoundingRectangle.DifferenceWithPoint(point);
          var diff2 = leaf2.BoundingRectangle.DifferenceWithPoint(point);
          var diff = Math.Abs(diff1 - diff2);

          if (maxDiff == null || diff > maxDiff.Value)
          {
            maxDiff = diff;
            maxPoint = point;
            if (diff2 > diff1)
              toFirst = true;
            else if (diff1 > diff2)
              toFirst = false;
            else if (leaf1.BoundingRectangle.Area < leaf2.BoundingRectangle.Area)
              toFirst = true;
            else
              toFirst = false;
          }
        }

        if(toFirst)
        {
          leaf1.Insert(maxPoint);
          difference++;
        }
        else
        {
          leaf2.Insert(maxPoint);
          difference--;
        }

        leftovers.Remove(maxPoint);

        if (Math.Abs(difference) >= leftovers.Count)
        {
          if (difference > 0)
            leaf2.Insert(leftovers.ToArray());
          else
            leaf1.Insert(leftovers.ToArray());

          break;
        }
      }

      return (leaf1, leaf2);
    }

    private (Point, Point, HashSet<Point>) FindPivots(HashSet<Point> points)
    {
      (Point Outer, Point Inner) = (null, null);

      double? maxArea = null;

      var pointArray = new Point[points.Count];
      points.CopyTo(pointArray);

      for (int i = 0; i < pointArray.Length; i++)
      {
        for (int j = i + 1; j < pointArray.Length; j++)
        {
          var rect = new Rectangle(pointArray[i]);
          rect.AddPoint(pointArray[j]);

          if (!maxArea.HasValue || rect.Area > maxArea.Value)
          {
            Outer = pointArray[i];
            Inner = pointArray[j];
            maxArea = rect.Area;
          }
        }
      }

      var leftovers = new HashSet<Point>(pointArray);

      leftovers.Remove(Inner);
      leftovers.Remove(Outer);

      return (Inner, Outer, leftovers);
    }
  }
}
