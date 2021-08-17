using System;

namespace Vwm.RTree.Api.Configs
{
  public class DataGenConfig
  {
    public bool Double { get; set; } = false;
    public int Count { get; set; } = 0;
    public string SnapshotFile { get; set; } = null;

    public void WriteProgress(int i)
    {
      if (i == 0)
      {
        Console.Write("Generate data progress: 0%");
      }
      else if ((i + 1) == Count)
      {
        Console.CursorLeft = Math.Max(0, Console.CursorLeft - 3);
        Console.Write("");
        Console.WriteLine("100%");
      }
      else if ((i * 100 / Count) != 0 && (i * 100 / Count % 2) == 0 && ((i * 100 / Count) < ((i + 1) * 100 / Count)))
      {
        if (((i * 100) / Count) > 10)
        {
          Console.CursorLeft = Math.Max(0, Console.CursorLeft - 3);
          Console.Write("");
        }
        else
        {
          Console.CursorLeft = Math.Max(0, Console.CursorLeft - 2);
          Console.Write("");
        }
        Console.Write(i * 100 / Count + "%");
      }
    }
  }
}
