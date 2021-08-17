using System;
using Vwm.RTree.Api.Models;
using Vwm.RTree.Api.Models.RTree;

namespace Vwm.RTree.Api.Extensions
{
  public static class ModelExtensions
  {
    public static Point ToPoint(this AddPointModel model)
    {
      if (model == null)
        throw new ArgumentNullException(nameof(model));

      return new Point(model.Coords);
    }
  }
}
