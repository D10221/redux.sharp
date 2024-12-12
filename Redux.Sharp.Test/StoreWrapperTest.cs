
using Redux.Actions;
using Redux.Extra;

namespace Redux.Test;

[TestClass]
public class StoreWrapperTest
{
  [TestMethod]
  public void Test1()
  {
    var changed = false;
    var count = 0;
    var store = new MyStore(new() { Value = 0 });
    store.Use(s => next => action =>
    {
      count++;
      return next(action);
    });
    store.Subscribe(_ => changed = true);
    Assert.AreEqual(0, store.State.Value);

    store.Increment();
    Assert.AreEqual(1, store.State.Value);
    Assert.IsTrue(changed);

    changed = false;
    store.Dispatch(MyStore.inc);
    Assert.AreEqual(2, store.State.Value);
    Assert.IsTrue(changed);

    changed = false;
    store.Dispatch(MyStore.dec);
    Assert.AreEqual(1, store.State.Value);
    Assert.IsTrue(changed);

    changed = false;
    store.Add(5);
    Assert.AreEqual(6, store.State.Value);
    Assert.IsTrue(changed);

    changed = false;
    store.Multiply(2);
    Assert.AreEqual(12, store.State.Value);
    Assert.IsTrue(changed);

    changed = false;
    store.Divide.Invoke(3);
    Assert.AreEqual(4, store.State.Value);
    Assert.IsTrue(changed);
  }

  class MyStore(MyStore.MyState state) : StoreWrapper<MyStore, MyStore.MyState>(state)
  {
    public class MyState
    {
      public int? Value { get; set; }
    }

    #region Increment
    public const string inc = "inc";
    public void Increment() => Dispatch(inc);
    #endregion

    #region Decrement
    public const string dec = "dec";
    public void Decrement() => Dispatch(dec);
    #endregion

    #region Add
    public const string add = "add";
    public void Add(int x) => Dispatch(ActionFactory.CreateAction(add, x));
    #endregion

    #region Multiply
    /// <summary>
    /// Field
    /// </summary>
    readonly IActionCreator<int> multiply = ActionFactory.Create<int>("multiply");
    public void Multiply(int x) => Dispatch(multiply.Create(x));
    #endregion

    #region Divide
    /// <summary>
    /// Property
    /// </summary>
    public IDispatcher<int> Divide => field ??= ActionFactory.Create<int>("divide", Dispatch);
    #endregion

    protected override MyState Reducer(MyState state, object action)
    {
      return action switch
      {
        inc => new() { Value = state.Value + 1 },
        dec => new() { Value = state.Value - 1 },
        IAction a when a.IsType(add) => new() { Value = state.Value + (int)a.Payload[0] },
        IAction a when a.IsType(multiply.Type) => new() { Value = state.Value * (int)a.Payload[0] },
        IAction a when a.IsType(Divide.Type) => 
          new() { Value = state.Value / (int)a.Payload[0] },
        _ => state
      };
    }    
  }
}
