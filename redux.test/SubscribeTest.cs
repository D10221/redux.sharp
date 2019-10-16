using System;
using FluentAssertions;
using Redux;
using Xunit;

namespace redux.test
{
  using static Store;
  using static Reducers;
  using static Actions;

  public class SubscribeTest
  {
    [Fact]
    public void ItWorks()
    {
      var initialState = new { a = false, b = false, actionCount = 0 };

      string A = Guid.NewGuid().ToString();
      string B = Guid.NewGuid().ToString();

      var reducer = CreateReducer((state, action) =>
      {
        switch (action)
        {
          case IAction a when Equals(a.Type, A):
            {
              dynamic _state = state;
              return new
              {
                a = true,
                b = (bool)_state.b,
                actionCount = (int)_state.actionCount + 1
              };
            }
          case IAction a when Equals(a.Type, B):
            {
              dynamic _state = state;
              return new
              {
                b = true,
                a = (bool)_state.a,
                actionCount = (int)_state.actionCount + 1
              };
            }
          default: return state;
        }
      }, initialState);

      var store = CreateStore(reducer, initialState);

      var dispatchCount = 0;
      void onDispatched()
      {
        ++dispatchCount;
      }

      using (var disposable = store.subscribe(onDispatched))
      {
        store.dispatch(CreateAction(A));
      }
      
      dispatchCount.Should().Be(1);

      store.dispatch(CreateAction(B));

      dispatchCount.Should().Be(1);
      
      dynamic s = store.getState();
      ((bool)s.a).Should().Be(true);
      ((bool)s.b).Should().Be(true);
      ((int)s.actionCount).Should().Be(2);
      
    }
  }
}