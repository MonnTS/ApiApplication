using Microsoft.Extensions.Logging;
using RestWebApp.Contracts;

namespace RestWebApp.LoggerService;

public class LoggerManager : ILoggerManager
{
    private readonly ILogger<LoggerManager> _logger;

    public LoggerManager(ILogger<LoggerManager> logger)
    {
        _logger = logger;
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(message);
    }

    public void LogError(string message)
    {
        _logger.LogError(message);
    }

    public void LogInfo(string message)
    {
        _logger.LogInformation(message);
    }

    public void LogWarn(string message)
    {
        _logger.LogWarning(message);
    }
}