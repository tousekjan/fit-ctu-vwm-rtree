using Newtonsoft.Json;
using System;

namespace Vwm.RTree
{
  public class Point: IEquatable<Point>
  {
    public double[] Coords { get; }

    public Point(double[] coordinates)
    {
      if (coordinates == null)
        throw new ArgumentNullException(nameof(coordinates));

      if (coordinates.Length != Constants._Dimensions)
        throw new ArgumentOutOfRangeException(nameof(coordinates));

      Coords = coordinates;
    }

    public override bool Equals(object obj)
    {
      return obj is Point point && Equals(point);
    }

    public bool Equals(Point other)
    {
      for (int i = 0; i < Constants._Dimensions; i++)
      {
        if (other.Coords[i] != Coords[i])
          return false;
      }

      return true;
    }

    public override int GetHashCode()
    {
      int hash = -319820321;

      foreach (double coord in Coords)
        hash += coord.GetHashCode();


      return hash;
    }

    public SnapshotPoint ToSnapshot()
    {
      return new SnapshotPoint
      {
        Coords = Coords
      };
    }
  }
}
