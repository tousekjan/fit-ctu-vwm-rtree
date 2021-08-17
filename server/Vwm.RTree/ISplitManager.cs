using System.Collections.Generic;

namespace Vwm.RTree
{
  public interface ISplitManager
  {
    (LeafNode, LeafNode) SplitLeaf(HashSet<Point> points, int id1, int id2, Point newPoint);
    void SplitInner(
      Stack<InnerNode> nodeChain, 
      InnerNode node, 
      NodeBase insertedNode1, 
      NodeBase insertedNode2, 
      NodeCache nodeCache,
      ref int rootNodeId,
      ref int nextNodeId);
  }
}
