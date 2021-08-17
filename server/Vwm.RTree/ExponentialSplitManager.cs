using System;
using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree
{
  public class ExponentialSplitManager : ISplitManager
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

      var id1 = node.Id;
      var id2 = nextNodeId++;

      var nodes = new List<NodeBase> { insertedNode2 };

      foreach (int id in node.ChildrenIds)
      {
        nodes.Add(nodeCache.GetById(id));
      }

      var combinations = Combs(nodes.ToArray(), nodes.Count).ToArray();

      double? minArea = null;
      (InnerNode, InnerNode)? newNodes = null;

      foreach (var comb in combinations)
      {
        var combArray = comb.ToArray();
        var newNode1 = new InnerNode(id1);
        var newNode2 = new InnerNode(id2);
        int i = 0;
        for (; i < nodes.Count; i++)
        {
          if (i < nodes.Count / 2)
            newNode1.Insert(combArray[i]);
          else
            newNode2.Insert(combArray[i]);
        }

        var area = newNode1.BoundingRectangle.Area + newNode2.BoundingRectangle.Area
          - newNode1.BoundingRectangle.Copy().Intersect(newNode2.BoundingRectangle).Area;
        if (minArea == null || minArea.Value > area)
        {
          minArea = area;
          newNodes = (newNode1, newNode2);
        }
      }

      nodeCache[newNodes.Value.Item1.Id] = newNodes.Value.Item1;
      nodeCache[newNodes.Value.Item2.Id] = newNodes.Value.Item2;

      if (nodeChain.Any())
      {
        var parent = nodeChain.Pop();
#warning mozna spatne, vyresit ASAP
        parent.Insert(newNodes.Value.Item1);
        parent.Insert(newNodes.Value.Item2);
        SplitInner(nodeChain, parent, newNodes.Value.Item1, newNodes.Value.Item2, nodeCache, ref rootNodeId, ref nextNodeId);
      }
      else
      {
        var newRoot = new InnerNode(nextNodeId++);
        rootNodeId = newRoot.Id;
        newRoot.Insert(newNodes.Value.Item1);
        newRoot.Insert(newNodes.Value.Item2);

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
      var pointArr = points.ToArray();
      var combinations = Combs(pointArr, pointArr.Length).ToArray();
      double? minArea = null;
      (LeafNode, LeafNode)? nodes = null;

      for (int j = 0; j < combinations.Length; j++)
      {
        var combArray = combinations[j].ToArray();
        var newNode1 = new LeafNode(id1);
        var newNode2 = new LeafNode(id2);
        int i = 0;
        for (; i < combArray.Length; i++)
        {
          if (i < combArray.Length / 2)
            newNode1.Insert(combArray[i]);
          else
            newNode2.Insert(combArray[i]);
        }

        var area = newNode1.BoundingRectangle.Area + newNode2.BoundingRectangle.Area
          - newNode1.BoundingRectangle.Copy().Intersect(newNode2.BoundingRectangle).Area;
        if (minArea == null || minArea.Value > area)
        {
          minArea = area;
          nodes = (newNode1, newNode2);
        }
      }

      return nodes.Value;
    }

    private static IEnumerable<int[]> Combos(int m, int n)
    {
      int[] result = new int[m];
      Stack<int> stack = new Stack<int>(m);
      stack.Push(0);
      while (stack.Count > 0)
      {
        int index = stack.Count - 1;
        int value = stack.Pop();
        while (value < n)
        {
          result[index++] = value++;
          stack.Push(value);
          if (index != m) continue;
          yield return (int[])result.Clone(); 
                                              
          break;
        }
      }
    }

    public static IEnumerable<T[]> Combs<T>(T[] array, int m)
    {
      if (array.Length < m)
        throw new ArgumentException("Array length can't be less than number of selected elements");
      if (m < 1)
        throw new ArgumentException("Number of selected elements can't be less than 1");
      T[] result = new T[m];
      foreach (int[] j in Combos(m, array.Length))
      {
        
        for (int i = 0; i < m; i++)
        {
          result[i] = array[j[i]];
        }
        yield return result;
      }
    }
  }
}
