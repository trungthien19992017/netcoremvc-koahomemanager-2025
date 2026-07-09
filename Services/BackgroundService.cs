namespace KOAHome.Services
{
  public class QueuedHostedService : BackgroundService
  {
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger)
    {
      _taskQueue = taskQueue;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          var workItem = await _taskQueue.DequeueAsync(stoppingToken);
          await workItem(stoppingToken);
        }
        catch (OperationCanceledException)
        {
          break; // app đang shutdown
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Lỗi khi xử lý background task");
        }
      }
    }
  }
}
