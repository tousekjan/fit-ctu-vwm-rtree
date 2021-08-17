using System.Collections.Generic;

namespace Vwm.RTree
{
  public class ReverseDoubleComparer : IComparer<double>
  {
    public int Compare(double x, double y)
    {
      if (x > y)
        return 1;

      if (x < y)
        return -1;

      return 0;
    }
  }
}
