using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Vwm.RTree.Api.Attributes;

namespace Vwm.RTree.Api.Models.Query
{
  public class KnnQueryModel
  {
    [Range(1, 200)]
    [Required, JsonRequired]
    public int Count { get; set; }

    [Required, JsonRequired]
    [LengthDimensionClamped]
    public double[] Coords { get; set; }
  }
}
