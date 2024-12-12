namespace Redux.Extra;
public interface ISlice<TStoreState>
{
    TStoreState Reducer(TStoreState state, object action);
}