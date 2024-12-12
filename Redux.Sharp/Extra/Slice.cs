using Redux.Extra;
namespace Redux;
public abstract class Slice<TStoreState, TSliceState>(IStore<TStoreState> store) : SubscriptionHandler, IStore<TSliceState>, ISlice<TStoreState>
{
  public abstract TSliceState State { get; protected set; }  
  public abstract TSliceState Reduce(TSliceState state, object action);
  protected TStoreState StoreState => store.State;  
  public TStoreState Reducer(TStoreState state, object action)
  {
    TSliceState current = State;
    TSliceState value = Reduce(current, action);
    if (!Equals(current, value)) OnChange();
    State = value;
    return state; ;
  }  
  public object Dispatch(object action) => store.Dispatch(action);
}
