using System.Threading.Channels;

namespace KOAHome.Services
{
  public interface IBackgroundTaskQueue
  {
    ValueTask QueueAsync(Func<CancellationToken, Task> workItem);
    ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct);
  }

  public class BackgroundTaskQueue : IBackgroundTaskQueue
  {
    private readonly Channel<Func<CancellationToken, Task>> _queue;

    public BackgroundTaskQueue(int capacity = 200)
    {
      var options = new BoundedChannelOptions(capacity)
      {
        FullMode = BoundedChannelFullMode.Wait // nếu queue đầy, chờ thay vì mất job
      };
      _queue = Channel.CreateBounded<Func<CancellationToken, Task>>(options);
    }

    public async ValueTask QueueAsync(Func<CancellationToken, Task> workItem)
    {
      await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct)
    {
      return await _queue.Reader.ReadAsync(ct);
    }
  }
}
