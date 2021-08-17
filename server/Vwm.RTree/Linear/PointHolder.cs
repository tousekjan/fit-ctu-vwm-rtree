using System.Collections.Generic;
using System.Linq;

namespace Vwm.RTree.Linear
{
  public class PointHolder
  {
    private HashSet<Point> fPoints;

    public PointHolder()
    {
      fPoints = new HashSet<Point>();
    }

    public void Replace(Point[] points)
    {
      fPoints = new HashSet<Point>(points);
    }

    public void Insert(Point point)
    {
      fPoints.Add(point);
    }

    public void Clean()
    {
      fPoints.Clear();
    }

    public IEnumerable<Point> RangeQuery(Point middle, double range)
    {
      foreach (var point in fPoints)
        if (MathUtils.Distance(middle, point) <= range)
          yield return point;
    }

    public Point[] KnnQuery(Point middle, int count)
    {
      var nearest = new SortedList<(double,Point)>(count);

      foreach (var point in fPoints)
      {
        double distance = MathUtils.Distance(point, middle);

        nearest.TryAdd((distance, point), new PointDistanceComparer());
      }

      return nearest.Items.Select(x => x.Item2).ToArray();
    }

    public Point NnQuery(Point middle)
    {
      Point nearest = null;
      double? minDistance = null;

      foreach (var point in fPoints)
      {
        double distance = MathUtils.Distance(point, middle);

        if (minDistance == null || distance < minDistance.Value)
        {
          nearest = point;
          minDistance = distance;
        }
      }

      return nearest;
    }
  }
}
