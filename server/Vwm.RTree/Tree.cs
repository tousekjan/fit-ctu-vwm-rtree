using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vwm.RTree
{
  public class Tree
  {
    private readonly ILogger fLogger;
    private NodeCache fNodeCache;

    private ISplitManager fSplitManager;

    private int fRootNodeId;
    public int RootNodeId { get => fRootNodeId; private set => fRootNodeId = value; }
    private int fNextNodeId = 0;

    public IEnumerable<NodeBase> Nodes => fNodeCache.Nodes;

    public Tree(ILogger logger)
    {
      CleanCacheDiectory();
      fSplitManager = GetSplitManager();
      fLogger = logger ?? throw new ArgumentNullException(nameof(logger));
      fNodeCache = new NodeCache();

      RootNodeId = fNextNodeId++;
      var root = new InnerNode(RootNodeId);
      var leaf = new LeafNode(fNextNodeId++);
      root.ChildrenIds.Add(leaf.Id);

      fNodeCache.Add(root);
      fNodeCache.Add(leaf);
    }

    public void FillFromSnapshot(Snapshot snapshot)
    {
      RootNodeId = snapshot.RootNodeId;
      fNextNodeId = snapshot.NextNodeId;

      fNodeCache = new NodeCache();
      fSplitManager = GetSplitManager();

      foreach (var x in snapshot.Nodes)
      {
        fNodeCache.Add(x.ToNode());
      }

      fLogger.LogInformation("Tree filled from snapshot!");
    }

    private void CleanCacheDiectory()
    {
      var dir = Directory.CreateDirectory("Cache");

      foreach (var file in dir.EnumerateFiles())
        file.Delete();
    }

    public void Clean()
    {
      CleanCacheDiectory();
      fSplitManager = GetSplitManager();
      fNodeCache = new NodeCache();

      RootNodeId = 0;
      fNextNodeId = 0;
      RootNodeId = fNextNodeId++;
      var root = new InnerNode(RootNodeId);
      var leaf = new LeafNode(fNextNodeId++);
      root.ChildrenIds.Add(leaf.Id);

      fNodeCache.Add(root);
      fNodeCache.Add(leaf);
    }

    private ISplitManager GetSplitManager()
    {
      switch (Constants._SplitType)
      {
        case SplitType.Linear:
          return new LinearSplitManager();
        case SplitType.Quadratic:
          return new QuadraticSplitManager();
        case SplitType.Exponential:
          return new ExponentialSplitManager();
        default:
          throw new NotImplementedException(Constants._SplitType.ToString());
      }
    }

    public Snapshot GetSnapshot()
    {
      return new Snapshot
      {
        NextNodeId = fNextNodeId,
        RootNodeId = RootNodeId,
        Dimensions = Constants._Dimensions,
        MaxNumberOfChildren = Constants._MaxNumberOfChildren,
        MaxPointCount = Constants._MaxPointCount,
        MinNumberOfChildren = Constants._MinNumberOfChildren,
        CacheSize = Constants._CacheSize,
        SplitType = Constants._SplitType,
        Nodes = fNodeCache.Nodes.Select(x => x.ToSnapshot()).ToArray()
      };
    }

    public override string ToString()
    {
      return $"RTree";
    }

    public void Insert(Point point)
    {
      var currentNode = fNodeCache[RootNodeId];
      var nodeChain = new Stack<InnerNode>();

      while (true)
      {
        if (currentNode is InnerNode innerNode)
        {
          double? leastResizedArea = null;
          var leastResized = new List<NodeBase>(innerNode.ChildrenIds.Count);

          foreach (int id in innerNode.ChildrenIds)
          {
            var child = fNodeCache[id];
            double resizedArea = child.DifferenceWithPoint(point);

            if (!leastResizedArea.HasValue || leastResizedArea.Value >= resizedArea)
            {
              if (leastResizedArea.HasValue && leastResizedArea.Value > resizedArea)
                leastResized.Clear();

              leastResized.Add(child);

              leastResizedArea = resizedArea;
            }
          }

          var leastResizedNode = leastResized
            .OrderByDescending(x => x.BoundingRectangle?.Area ?? 0)
            .First();

          nodeChain.Push(innerNode);
          currentNode = fNodeCache[leastResizedNode.Id];
        }
        else
        {
          var leafNode = currentNode as LeafNode;

          if (leafNode.IsFull)
          {
            int id = fNextNodeId++;

            var (leaf1, leaf2) = leafNode.Split(id, point, fSplitManager);

            fNodeCache.Add(leaf1);
            fNodeCache.Add(leaf2);

            var parent = nodeChain.Pop();

            fSplitManager.SplitInner(nodeChain, parent, leaf1, leaf2, fNodeCache, ref fRootNodeId, ref fNextNodeId);
          }
          else
          {
            leafNode.Insert(point);

            foreach (var node in nodeChain)
              node.Insert(point);
          }

          break;
        }
      }
    }
    public bool Contains(Point point)
    {
      var rootNode = GetRootNode();
      if (rootNode.BoundingRectangle == null)
        return false;

      var nodeQueue = new Queue<NodeBase>();
      nodeQueue.Enqueue(rootNode);

      while (nodeQueue.Any())
      {
        var currentNode = nodeQueue.Dequeue();

        if (currentNode is InnerNode innerNode)
        {
          foreach (int id in innerNode.ChildrenIds)
          {
            var childNode = fNodeCache.GetById(id);
            if (childNode.BoundingRectangle.Contains(point))
              nodeQueue.Enqueue(childNode);
          }
        }
        else
        {
          var leafNode = currentNode as LeafNode;
          foreach (var p in leafNode.Points)
          {
            if (p.Equals(point))
              return true;
          }
        }
      }

      return false;
    }

    public Point[] KNNQuery(Point point, int count)
    {
      var rootNode = GetRootNode();
      if (rootNode.BoundingRectangle == null)
        return null;

      var nodes = new Queue<NodeBase>();
      foreach (var id in rootNode.ChildrenIds)
      {
        var node = fNodeCache.GetById(id);
        nodes.Enqueue(fNodeCache.GetById(id));
      }

      while (true)
      {
        var minMinMaxDists = new SortedList<double>(count);
        foreach (var node in nodes)
        {
          var minMaxDist = MathUtils.MinMaxDistance(point, node.BoundingRectangle);
          minMinMaxDists.TryAdd(minMaxDist);
        }

        var filtered = nodes.Where(x => MathUtils.MinDistance(point, x.BoundingRectangle) <= minMinMaxDists.Last).ToArray();
        if (nodes.Peek() is LeafNode)
        {
          var minDists = new SortedList<(double, Point)>(count);
          foreach (var node in filtered)
          {
            var leaf = node as LeafNode;
            foreach (var p in leaf.Points)
            {
              var dist = MathUtils.Distance(point, p);
              minDists.TryAdd((dist, p), new PointDistanceComparer());
            }
          }
          return minDists.Items.Select(x => x.Item2).ToArray();
        }

        nodes.Clear();
        foreach (var node in filtered)
        {
          var inner = node as InnerNode;
          foreach (var id in inner.ChildrenIds)
            nodes.Enqueue(fNodeCache.GetById(id));
        }
      }
    }

    public Point NNQuery(Point point)
    {
      var rootNode = GetRootNode();
      if (rootNode.BoundingRectangle == null)
        return null;

      var nodes = new Queue<NodeBase>();
      foreach (var id in rootNode.ChildrenIds)
      {
        var node = fNodeCache.GetById(id);
        nodes.Enqueue(fNodeCache.GetById(id));
      }

      while (true)
      {
        double? minMinMaxDist = null;
        foreach (var node in nodes)
        {
          var minMaxDist = MathUtils.MinMaxDistance(point, node.BoundingRectangle);
          if (minMinMaxDist == null || minMaxDist < minMinMaxDist)
            minMinMaxDist = minMaxDist;
        }

        var filtered = nodes.Where(x => MathUtils.MinDistance(point, x.BoundingRectangle) <= minMinMaxDist.Value).ToArray();
        if (nodes.Peek() is LeafNode)
        {
          double? minDist = null;
          Point minPoint = null;
          foreach (var node in filtered)
          {
            var leaf = node as LeafNode;
            foreach (var p in leaf.Points)
            {
              var dist = MathUtils.Distance(point, p);
              if (minDist == null || dist < minDist.Value)
              {
                minDist = dist;
                minPoint = p;
              }
            }
          }
          return minPoint;
        }

        nodes.Clear();
        foreach (var node in filtered)
        {
          var inner = node as InnerNode;
          foreach (var id in inner.ChildrenIds)
            nodes.Enqueue(fNodeCache.GetById(id));
        }
      }
    }

    public IEnumerable<Point> RangeQuery(Point middle, double range)
    {
      var rootNode = GetRootNode();
      if (rootNode.BoundingRectangle == null)
        return Array.Empty<Point>();

      var queryRectangle = Rectangle.GetRectangle(middle, range);
      return RangeQuery(middle, range, queryRectangle, rootNode).Distinct();
    }

    private IEnumerable<Point> RangeQuery(Point middle, double range, Rectangle queryRectangle, NodeBase currentNode)
    {
      if (currentNode is InnerNode innerNode)
      {
        foreach (int id in innerNode.ChildrenIds)
        {
          var entry = fNodeCache.GetById(id);
          if (entry.BoundingRectangle.IntersectsWith(queryRectangle))
            foreach (var point in RangeQuery(middle, range, queryRectangle, entry))
              yield return point;
        }
      }
      else
      {
        var leafNode = currentNode as LeafNode;
        foreach (var point in leafNode.Points)
        {
          if (MathUtils.Distance(point, middle) <= range)
            yield return point;
        }
      }
    }

    private InnerNode GetRootNode()
    {
      return fNodeCache.GetById(RootNodeId) as InnerNode;
    }
  }
}
