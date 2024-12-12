using Redux.Actions;
using Redux.Extra;

namespace Redux.Test;

public partial class MyStoreTest
{
  class MyStore(MyStore.MyState state) : SuperStore<MyStore, MyStore.MyState>(state)
  {
    public class SubState<T>
    {
      public T Value { get; set; }
    }
    public class MyState
    {
      public SubState<int> Sub1 { get; set; }
      public SubState<string> Sub2 { get; set; }
    }
    public SubStateSlice1 Sub1 => field ??= new SubStateSlice1(this);
    public SubStateSlice2 Sub2 => field ??= new SubStateSlice2(this);

    public class SubStateSlice1(MyStore store) : Slice<MyState, SubState<int>>(store)
    {
      public override SubState<int> State { get => StoreState.Sub1; protected set => StoreState.Sub1 = value; }
      public const string inc = $"@{nameof(SubStateSlice1)}/inc";
      public const string dec = $"@{nameof(SubStateSlice1)}/dec";
      public void Increment() => Dispatch(inc);
      public void Decrement() => Dispatch(dec);
      public override SubState<int> Reduce(SubState<int> state, object action)
      {
        return action switch
        {
          inc => new() { Value = state.Value + 1 },
          dec => new() { Value = state.Value - 1 },
          _ => state
        };
      }
    }
    public class SubStateSlice2(MyStore store) : Slice<MyState, SubState<string>>(store)
    {
      public override SubState<string> State { get => StoreState.Sub2; protected set => StoreState.Sub2 = value; }
      public IDispatcher<string> Add => field ??= ActionFactory.Create<string>(Guid.NewGuid(), Dispatch);
      public IDispatcher<string> Replace => field ??= ActionFactory.Create<string>(Guid.NewGuid(), Dispatch);
      public IDispatcher CopySub1 => field ??= ActionFactory.Create(nameof(CopySub1), Dispatch);
      public override SubState<string> Reduce(SubState<string> state, object action) => action switch
      {
        //
        IAction a when a.IsType(Add.Type) => state.Clone(x => x.Value = state.Value + (string)a.Payload[0]),
        //
        IAction a when a.IsType(Replace.Type) => state.Clone(x => x.Value = state.Value.Replace((string)a.Payload[0], "")),
        //
        IAction a when a.IsType(CopySub1.Type) => state.Clone(x => x.Value = store.Sub1.State.Value.ToString()),
        _ => state
      };
    }
  }
}
