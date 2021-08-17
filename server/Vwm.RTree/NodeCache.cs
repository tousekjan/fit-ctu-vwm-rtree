using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vwm.RTree
{
  public class NodeCache
  {
    private readonly Dictionary<int, NodeBase> fNodes;

    public IEnumerable<NodeBase> Nodes => fNodes.Values.Union(LoadAll()).Distinct();

    public NodeBase GetById(int id)
    {
      if (fNodes.TryGetValue(id % Constants._CacheSize, out var node) && node.Id == id)
        return node;

      Save(node);
      var tNode = Load(id);
      fNodes[id % Constants._CacheSize] = tNode;

      return tNode;
    }

    public void Add(NodeBase node)
    {
      if (fNodes.TryGetValue(node.Id % Constants._CacheSize, out var oldNode) && oldNode.Id != node.Id)
        Save(oldNode);

      fNodes[node.Id % Constants._CacheSize] = node;
    }

    public NodeBase this[int id]
    {
      get
      {
        if (fNodes.TryGetValue(id % Constants._CacheSize, out var node) && node.Id == id)
          return node;

        Save(node);
        var tNode = Load(id);
        fNodes[id % Constants._CacheSize] = tNode;

        return tNode;
      }
      set
      {
        if (fNodes.TryGetValue(id % Constants._CacheSize, out var oldNode) && oldNode.Id != value.Id)
          Save(oldNode);

        fNodes[id % Constants._CacheSize] = value;
      }
    }

    public NodeCache()
    {
      fNodes = new Dictionary<int, NodeBase>();
    }

    public void Save(NodeBase node)
    {
      var snap = node.ToSnapshot();
      File.WriteAllText($"Cache/{node.Id}.json", JsonConvert.SerializeObject(snap,
        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None }));
    }

    public NodeBase Load(int id)
    {
      var str = File.ReadAllText($"Cache/{id}.json");
      var snap = JsonConvert.DeserializeObject<SnapshotNode>(str,
        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });

      return snap.ToNode();
    }

    public IEnumerable<NodeBase> LoadAll()
    {
      var dir = Directory.CreateDirectory("Cache");

      foreach (var file in dir.EnumerateFiles())
      {
        var str = File.ReadAllText($"Cache/{file.Name}");
        var snap = JsonConvert.DeserializeObject<SnapshotNode>(str,
          new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });

        yield return snap.ToNode();
      }
    }
  }
}
