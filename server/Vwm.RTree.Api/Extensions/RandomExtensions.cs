using System;
using System.Collections.Generic;

namespace Vwm.RTree.Api.Extensions
{
  public static class RandomExtensions
  {
    public static IEnumerable<double> NextCount(this Random rand, int count)
    {
      for (int i = 0; i < count; i++)
        yield return rand.Next();
    }

    public static IEnumerable<double> NextDoubleCount(this Random rand, int count)
    {
      for (int i = 0; i < count; i++)
        yield return rand.NextDouble() * 1_000_000;
    }
  }
}
