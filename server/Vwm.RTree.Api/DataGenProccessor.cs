using System;
using System.Linq;
using Vwm.RTree.Api.Configs;
using Vwm.RTree.Api.Extensions;
using Vwm.RTree.Linear;

namespace Vwm.RTree.Api
{
  public static class DataGenProccessor
  {
    public static void Fill(DataGenConfig config, Tree rtree, PointHolder pointHolder)
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));
      if (rtree == null)
        throw new ArgumentNullException(nameof(rtree));
      if (pointHolder == null)
        throw new ArgumentNullException(nameof(pointHolder));

      var random = new Random(Environment.TickCount);
      if (config.Double)
      {
        for (int i = 0; i < config.Count; i++)
        {
          var point = new Point(random.NextDoubleCount(Constants._Dimensions).ToArray());
          rtree.Insert(point);
          pointHolder.Insert(point);
          config.WriteProgress(i);
        }
        return;
      }

      for (int i = 0; i < config.Count; i++)
      {
        var point = new Point(random.NextCount(Constants._Dimensions).ToArray());
        rtree.Insert(point);
        pointHolder.Insert(point);
        config.WriteProgress(i);
      }
    }

    public static void Fill(DataGenConfig config, Tree rtree)
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));
      if (rtree == null)
        throw new ArgumentNullException(nameof(rtree));

      var random = new Random(Environment.TickCount);

      if (config.Double)
        for (int i = 0; i < config.Count; i++)
        {
          rtree.Insert(new Point(random.NextDoubleCount(Constants._Dimensions).ToArray()));
          config.WriteProgress(i);
        }
      else
        for (int i = 0; i < config.Count; i++)
        {
          rtree.Insert(new Point(random.NextCount(Constants._Dimensions).ToArray()));
          config.WriteProgress(i);
        }
    }
  }
}
