namespace Redux;
public class Disposable(Action onDispose) : IDisposable
{
  public static IDisposable Create(Action onDispose) => new Disposable(onDispose);
  public void Dispose() => onDispose?.Invoke();
}
