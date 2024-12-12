using System.Reflection;
namespace Redux.Extra;
public class SuperStore<TSelf, TStoreState> : StoreWrapper<TSelf, TStoreState>
{
    private Reducer<TStoreState> reduce;
    public SuperStore(TStoreState state) : base(state)
    {
        reduce = Slices.Select(s => (Reducer<TStoreState>)s.Reducer).Combine();
    }
    IEnumerable<ISlice<TStoreState>> Slices => field ??=
      GetType()
      // Or Fields ?
      .GetProperties(BindingFlags.Instance | BindingFlags.Public)
      .Where(p => p.PropertyType.Implements(typeof(ISlice<TStoreState>)))
      .Select(p => (ISlice<TStoreState>)p.GetValue(this));

    IEnumerable<SubscriptionHandler> SubsriptionHandlers()
    {
        foreach (var s in Slices)
        {
            if (s is SubscriptionHandler sh)
                yield return sh;
        }
    }
    public override IDisposable Subscribe(Action<object> subscriber)
    {
        var disposables = SubsriptionHandlers().Select(s => s.Subscribe(subscriber)).ToArray();
        return Disposable.Create(() =>
        {
            foreach (var d in disposables)
            {
                d.Dispose();
            }
        });
    }
    protected override TStoreState Reducer(TStoreState state, object action) => reduce.Invoke(state, action);
}