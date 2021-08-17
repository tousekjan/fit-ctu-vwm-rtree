using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Vwm.RTree
{
  public class Rectangle
  {
    [JsonIgnore]
    public double Area { get; private set; }

    public (double Minimum, double Maximum)[] Coords { get; private set; }

    [JsonConstructor]
    public Rectangle((double min, double max)[] coords)
    {
      if (coords.Length != Constants._Dimensions)
        throw new ArgumentOutOfRangeException(nameof(coords));

      Coords = coords;
      CountArea();
    }

    public Rectangle(Point point)
    {
      Coords = point.Coords.Select(x => (x, x)).ToArray();
      CountArea();
    }

    public bool Contains(Rectangle rectangle)
    {
      for (int i = 0; i < Constants._Dimensions; i++)
      {
        var (Minimum, Maximum) = Coords[i];

        if (Maximum < rectangle.Coords[i].Maximum
          || Minimum > rectangle.Coords[i].Minimum)
          return false;
      }

      return true;
    }

    public void AddPoint(Point point)
    {
      for (int i = 0; i < Constants._Dimensions; i++)
      {
        Coords[i].Maximum = Max(Coords[i].Maximum, point.Coords[i]);
        Coords[i].Minimum = Min(Coords[i].Minimum, point.Coords[i]);
      }

      CountArea();
    }

    public double DifferenceWithPoint(Point point)
    {
      var newCoords = new (double, double)[Coords.Length];
      Coords.CopyTo(newCoords, 0);

      var resizedRect = new Rectangle(newCoords);
      resizedRect.AddPoint(point);

      return resizedRect.Area - Area;
    }

    public void Merge(Rectangle rec2)
    {
      var coords = new (double Minimum, double Maximum)[Constants._Dimensions];

      for (var i = 0; i < Constants._Dimensions; i++)
      {
        (double min1, double max1) = Coords[i];
        (double min2, double max2) = rec2.Coords[i];

        coords[i] = (Min(min1, min2), Max(max1, max2));
      }

      Coords = coords;
      CountArea();
    }

    public double DifferenceWithRectangle(Rectangle boundingRectangle)
    {
      var newCoords = new (double, double)[Coords.Length];
      Coords.CopyTo(newCoords, 0);

      var enlargedRectangle = new Rectangle(newCoords);
      enlargedRectangle.Merge(boundingRectangle);

      return Abs(Area - enlargedRectangle.Area);
    }

    public Rectangle Copy()
    {
      var newCoords = new (double, double)[Coords.Length];
      Coords.CopyTo(newCoords, 0);

      return new Rectangle(newCoords);
    }

    public bool IntersectsWith(Rectangle rectangle)
    {
      for (var i = 0; i < Constants._Dimensions; i++)
      {
        (double min1, double max1) = Coords[i];
        (double min2, double max2) = rectangle.Coords[i];

        if (min1 > max2 || min2 > max1)
        {
          return false;
        }
      }

      return true;
    }

    public Rectangle Intersect(Rectangle other)
    {
      (double min, double max)[] coords = new (double min, double max)[Constants._Dimensions];

      for (var i = 0; i < Constants._Dimensions; i++)
        coords[i] = (Max(Coords[i].Minimum, other.Coords[i].Maximum), Min(Coords[i].Maximum, other.Coords[i].Maximum));

      return new Rectangle(coords);
    }

    public bool Contains(Point point)
    {
      for (var i = 0; i < Constants._Dimensions; i++)
      {
        var (min, max) = Coords[i];

        if (point.Coords[i] > max || point.Coords[i] < min)
          return false;
      }

      return true;
    }

    public static Rectangle GetRectangle(Point middle, double range)
    {
      var coords = new (double Minimum, double Maximum)[Constants._Dimensions];

      for (var i = 0; i < Constants._Dimensions; i++)
        coords[i] = (middle.Coords[i] - range, middle.Coords[i] + range);

      return new Rectangle(coords);
    }

    private void CountArea()
    {
      double size = 1d;
      for (int i = 0; i < Constants._Dimensions; i++)
        size *= Abs(Coords[i].Maximum - Coords[i].Minimum);

      Area = size;
    }

    public IEnumerable<(double, double)> GetEndPoints()
    {
      for (var i = 0; i < Constants._Dimensions; i++)
        yield return (Coords[i].Minimum, Coords[i].Maximum);
    }

    public SnapshotRectangle ToSnapshot()
    {
      return new SnapshotRectangle
      {
        Coords = Coords
      };
    }
  }
}
