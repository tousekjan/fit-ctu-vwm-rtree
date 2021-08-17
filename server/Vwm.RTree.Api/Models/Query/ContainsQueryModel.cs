using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Vwm.RTree.Api.Attributes;

namespace Vwm.RTree.Api.Models.Query
{
  public class ContainsQueryModel
  {
    [Required, JsonRequired]
    [LengthDimensionClamped]
    public double[] Coords { get; set; }
  }
}
