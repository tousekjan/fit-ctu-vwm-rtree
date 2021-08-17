namespace Vwm.RTree
{
  public static class Constants
  {
    public static int _Dimensions { get; set; } = 2;

    public static int _MaxNumberOfChildren { get; set; } = 10;
    public static int _MinNumberOfChildren { get; set; } = _MaxNumberOfChildren / 2;

    public static int _MaxPointCount { get; set; } = 70;
    public static int _CacheSize { get; set; } = 500_000_000;

    public static SplitType _SplitType { get; set; } = SplitType.Linear;
  }
}
