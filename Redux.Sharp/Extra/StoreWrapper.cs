namespace Redux.Extra;
public abstract class StoreWrapper<TSelf, TState>(TState state = default, params Middleware<TState>[] middleware) : IStore<TState>
{    
    protected abstract TState Reducer(TState state, object action);
    protected Store<TState> Store
    {
        get => field ??= new Store<TState>(Reducer, state, middleware);
        private set => field = value;
    }
    public virtual TState State => Store.State;
    public virtual object Dispatch(object action) => Store.Dispatch(action);
    public virtual IDisposable Subscribe(Action<object> subscriber) => Store.Subscribe(subscriber);
    public void Use(params Middleware<TState>[] m) => Store = new Store<TState>(Reducer, Store.State, [.. middleware, .. m]);
}
