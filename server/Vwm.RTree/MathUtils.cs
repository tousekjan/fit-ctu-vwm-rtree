using Newtonsoft.Json;
using System;
using System.Linq;
using static System.Math;

namespace Vwm.RTree
{
  public static class MathUtils
  {
    public static double Distance(Point p1, Point p2)
    {
      double acc = 0;
      for (int i = 0; i < Constants._Dimensions; i++)
      {
        acc += Pow(p1.Coords[i] - p2.Coords[i], 2);
      }

      return Sqrt(acc);
    }

    /// <summary>
    /// MINDIST(Q, R) calculates the minimal distance From point Q to Rectangle R
    /// Definition: R-TreesTheoryAndApplications 4.3.1
    /// </summary>
    /// <param name="point"></param>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public static double MinDistance(Point point, Rectangle rectangle)
    {
      double result = 0;
      (double Min, double Max)[] endPoints = rectangle.GetEndPoints().ToArray();

      for (int i = 0; i < Constants._Dimensions; i++)
      {
        double coord = point.Coords[i];
        double min = endPoints[i].Min;
        double max = endPoints[i].Max;
        double r;

        if (coord < min)
          r = min;
        else if (coord > max)
          r = max;
        else
          r = coord;

        result += Pow(Abs(coord - r), 2);
      }

      return Sqrt(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public static double MaxDistance(Point point, Rectangle rectangle)
    {
      double result = 0;
      (double Min, double Max)[] endPoints = rectangle.GetEndPoints().ToArray();

      for (int i = 0; i < Constants._Dimensions; i++)
      {
        var a = Max(Abs(point.Coords[i] - endPoints[i].Min), Abs(point.Coords[i] - endPoints[i].Max));
        result += Pow(a, 2);
      }

      return Sqrt(result);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public static double MinMaxDistance(Point point, Rectangle rectangle)
    {
      (double Min, double Max)[] endPoints = rectangle.GetEndPoints().ToArray();
      double[] coords = new double[Constants._Dimensions];

      for (int k = 0; k < Constants._Dimensions; k++)
      {
        if (Abs(point.Coords[k] - endPoints[k].Min) > Abs(point.Coords[k] - endPoints[k].Max))
          coords[k] = endPoints[k].Min;
        else
          coords[k] = endPoints[k].Max;
      }
      double? min = null;

      for (int i = 0; i < Constants._Dimensions; i++)
      {
        if (coords[i] == endPoints[i].Min)
        {
          coords[i] = endPoints[i].Max;
          var dist = Distance(point, new Point(coords));
          if (min == null || min > dist)
            min = dist;
          coords[i] = endPoints[i].Min;
        }
        else
        {
          coords[i] = endPoints[i].Min;
          var dist = Distance(point, new Point(coords));
          if (min == null || min > dist)
            min = dist;
          coords[i] = endPoints[i].Max;
        }
      }
      return min.Value;
    }
  }
}
