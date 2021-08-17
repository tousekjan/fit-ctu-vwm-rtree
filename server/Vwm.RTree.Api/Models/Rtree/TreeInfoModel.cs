namespace Vwm.RTree.Api.Models.RTree
{
  public class TreeInfoModel
  {
    public int Dimensions => Constants._Dimensions;

    public  int MaxNumberOfChildren => Constants._MaxNumberOfChildren;

    public  int MinNumberOfChildren => Constants._MinNumberOfChildren;

    public  int MaxPointCount => Constants._MaxPointCount;

    public  int CacheSize => Constants._CacheSize;

    public  string SplitType => Constants._SplitType.ToString();
  }
}
