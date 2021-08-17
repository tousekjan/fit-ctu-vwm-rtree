using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Vwm.RTree.Api.Attributes;

namespace Vwm.RTree.Api.Models.Query
{
  public class RangeQueryModel
  {
    [Range(1, double.MaxValue)]
    [Required, JsonRequired]
    public double Range { get; set; }

    [Required, JsonRequired]
    [LengthDimensionClamped]
    public double[] Coords { get; set; }
  }
}
