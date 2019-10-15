using System;

namespace Redux
{
  class Disposable : IDisposable
  {
    public static Disposable Create(Action action)
    {
      return new Disposable(action);
    }
    readonly Action _action;

    public Disposable(Action action)
    {
      _action = action;
    }
    public void Dispose()
    {
      _action?.Invoke();
    }
  }
}