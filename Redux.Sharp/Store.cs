namespace Redux;
using System.Collections.Generic;
using System.Threading;
public delegate object Dispatcher(object action);
public delegate T Reducer<T>(T state, object action);
public delegate Func<Dispatcher, Dispatcher> Middleware<T>(IStore<T> api);
public interface IStore<T>
{
    T State { get; }
    object Dispatch(object action);
    IDisposable Subscribe(Action<object> subscriber);
}
public class Store<TState>(
    Reducer<TState> reducer,
    TState state = default,
    params Middleware<TState>[] middleware
    ) : SubscriptionHandler, IStore<TState>
{
    protected virtual Dispatcher dispatcher => field ??= middleware.ApplyMiddleware(this).Invoke(Dispatcher);
    readonly Lock @lock = new();
    /// <summary>
    /// Internal Dispatcher
    /// </summary>
    /// <param name="action"></param>
    /// <returns>action</returns>
    protected virtual TAction Dispatcher<TAction>(TAction action)
    {
        lock (@lock)
        {
            State = reducer.Invoke(State, action);
            return action;
        }
    }
    public virtual object Dispatch(object action) => dispatcher(action);
    public virtual TState State
    {
        get => field;
        protected set
        {
            if (!EqualityComparer<TState>.Default.Equals(field, value))
            {
                field = value;
                OnChange();
            }
        }
    } = state;

}
public class Store
{
    public static IStore<TState> From<TState>(
        Reducer<TState> reducer,
        TState state = default,
        params Middleware<TState>[] middleware
        )
    {
        var store = new Store<TState>(reducer, state, middleware);
        return store;
    }
}