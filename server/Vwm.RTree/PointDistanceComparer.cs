using System.Collections.Generic;

namespace Vwm.RTree
{
  public class PointDistanceComparer: IComparer<(double Distance, Point)>
  {
    public int Compare((double Distance, Point) x, (double Distance, Point) y)
    {
      if (x.Distance > y.Distance)
        return 1;

      if (x.Distance < y.Distance)
        return -1;

      return 0;
    }
  }
}
