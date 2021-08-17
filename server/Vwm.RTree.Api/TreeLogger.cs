using System;
using Microsoft.Extensions.Logging;

namespace Vwm.RTree.Api
{
  public class TreeLogger: ILogger
  {
    private readonly ILogger<Tree> fLogger;

    public TreeLogger(ILogger<Tree> logger)
    {
      fLogger = logger;
    }

    public void LogError(string text)
    {
      fLogger.LogError(text);
    }

    public void LogError(string text, Exception ex)
    {
      fLogger.LogInformation(text, ex);
    }

    public void LogInformation(string text)
    {
      fLogger.LogInformation(text);
    }

    public void LogWarning(string text)
    {
      fLogger.LogWarning(text);
    }
  }
}
