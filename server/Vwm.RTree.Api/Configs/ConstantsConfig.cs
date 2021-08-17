namespace Vwm.RTree.Api.Configs
{
  public class ConstantsConfig
  {
    public int Dimensions { get; set; }

    public int MaxNumberOfChildren { get; set; }
    public int MinNumberOfChildrenRatio { get; set; }

    public int MaxPointCount { get; set; }
    public int CacheSize { get; set; }

    public SplitType SplitType { get; set; }

    internal void Apply()
    {
      Constants._Dimensions = Dimensions;
      Constants._MaxNumberOfChildren = MaxNumberOfChildren;
      Constants._MinNumberOfChildren = MaxNumberOfChildren / MinNumberOfChildrenRatio;
      Constants._MaxPointCount = MaxPointCount;
      Constants._CacheSize = CacheSize;
      Constants._SplitType = SplitType;
    }
  }
}
