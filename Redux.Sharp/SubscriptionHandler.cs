namespace Redux;
using System;
public interface ISubscriptionHandler
{
    IDisposable Subscribe(Action<object> action);
}
public abstract class SubscriptionHandler : ISubscriptionHandler
{
    public event EventHandler Changed;
    protected void OnChange() => Changed?.Invoke(this, EventArgs.Empty);
    public IDisposable Subscribe(Action<object> action)
    {
        void handle(object s, EventArgs e) => action(this);
        Changed += handle;
        return new Disposable(() => Changed -= handle);
    }
}
