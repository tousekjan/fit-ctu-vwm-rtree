using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree
{
  public class SortedList<T>
  {
    public List<T> Items { get; }
    private readonly int fCount;

    public SortedList(int count)
    {
      Items = new List<T>(count + 1);
      fCount = count;
    }

    public void TryAdd(T item, IComparer<T> eq)
    {
      int index = Items.BinarySearch(item, eq);

      if (index < 0)
        index = ~index;

      Items.Insert(index, item);

      if (Items.Count > fCount)
        Items.RemoveAt(Items.Count - 1);
    }

    public T Last => Items[Items.Count - 1];

    public void TryAdd(T item)
    {
      int index = Items.BinarySearch(item);

      if (index < 0)
        index = ~index;

      Items.Insert(index, item);

      if (Items.Count > fCount)
        Items.RemoveAt(Items.Count - 1);
    }
  }
}
