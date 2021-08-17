using System;
using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree
{
  internal class LinearSplitManager : ISplitManager
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

      var nodes = new List<NodeBase> { insertedNode2 };

      foreach (int id in node.ChildrenIds)
      {
        nodes.Add(nodeCache.GetById(id));
      }

      var (farthestNode1, farthestNode2, leftovers) = GetFarthestRectangles(nodes);

      var newNode1 = new InnerNode(node.Id);
      var newNode2 = new InnerNode(nextNodeId++);

      newNode1.Insert(farthestNode1);
      newNode2.Insert(farthestNode2);


      while (leftovers.Any())
      {
        var currentNode = leftovers.Dequeue();

        double areaDifference1 = newNode1.BoundingRectangle.DifferenceWithRectangle(currentNode.BoundingRectangle);
        double areaDifference2 = newNode2.BoundingRectangle.DifferenceWithRectangle(currentNode.BoundingRectangle);

        nodeCache.Add(currentNode);

        if (areaDifference1 < areaDifference2)
          newNode1.Insert(currentNode);
        else if (areaDifference1 > areaDifference2)
          newNode2.Insert(currentNode);
        else
        {
          if (newNode1.ChildrenIds.Count > newNode2.ChildrenIds.Count)
            newNode2.Insert(currentNode);
          else
            newNode1.Insert(currentNode);
        }

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

    private static (NodeBase, NodeBase, Queue<NodeBase>) GetFarthestRectangles(List<NodeBase> nodes)
    {
      (NodeBase p1, NodeBase p2) = (null, null);

      var leftovers = new HashSet<NodeBase>(nodes);

      double? maxArea = null;

      for (int i = 0; i < nodes.Count; i++)
      {
        for (int j = i + 1; j < nodes.Count; j++)
        {
          double area = nodes[i].BoundingRectangle.DifferenceWithRectangle(nodes[j].BoundingRectangle);
          if (!maxArea.HasValue || area > maxArea.Value)
          {
            p1 = nodes[i];
            p2 = nodes[j];
            maxArea = area;
          }
        }
      }

      leftovers.Remove(p1);
      leftovers.Remove(p2);

      return (p1, p2, new Queue<NodeBase>(leftovers));
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

        var point = leftovers.Dequeue();

        if (leaf1.DifferenceWithPoint(point) < leaf2.DifferenceWithPoint(point))
        {
          difference++;
          leaf1.Insert(point);
        }
        else
        {
          difference--;
          leaf2.Insert(point);
        }

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

    private (Point, Point, Queue<Point>) FindPivots(HashSet<Point> points)
    {
      (Point Outer, Point Inner) = (null, null);

      double? maxDistance = null;

      var pointArray = new Point[points.Count];
      points.CopyTo(pointArray);

      for (int i = 0; i < pointArray.Length; i++)
      {
        for (int j = i + 1; j < pointArray.Length; j++)
        {
          double dist = MathUtils.Distance(pointArray[i], pointArray[j]);
          if (!maxDistance.HasValue || dist > maxDistance.Value)
          {
            Outer = pointArray[i];
            Inner = pointArray[j];
            maxDistance = dist;
          }
        }
      }

      var leftovers = new HashSet<Point>(pointArray);

      leftovers.Remove(Inner);
      leftovers.Remove(Outer);

      return (Inner, Outer, new Queue<Point>(leftovers));
    }
  }
}
