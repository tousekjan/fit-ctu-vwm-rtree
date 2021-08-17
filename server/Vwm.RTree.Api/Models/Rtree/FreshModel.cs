using System.ComponentModel.DataAnnotations;
using Vwm.RTree.Api.Attributes;
using Vwm.RTree.Api.Configs;

namespace Vwm.RTree.Api.Models.RTree
{
  public class FreshModel: DataGenConfig
  {
    [MinValue(2)]
    public int Dimensions { get; set; } = 2;

    [MinValue(2)]
    public int MaxNumberOfChildren { get; set; } = 10;

    [MinValue(2)]
    public int MinNumberOfChildrenRatio { get; set; } = 2;

    [MinValue(2)]
    public int MaxPointCount { get; set; } = 70;

    [MinValue(2)]
    public int CacheSize { get; set; } = 500_000_000;

    [EnumDataType(typeof(SplitType))]
    public SplitType SplitType { get; set; } = SplitType.Linear;
  }
}
