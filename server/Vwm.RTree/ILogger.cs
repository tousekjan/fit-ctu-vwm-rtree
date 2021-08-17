using System;

namespace Vwm.RTree
{
  public interface ILogger
  {
    void LogInformation(string text);
    void LogWarning(string text);
    void LogError(string text);
    void LogError(string text, Exception ex);
  }
}
