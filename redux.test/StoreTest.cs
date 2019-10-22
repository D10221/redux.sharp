using Xunit;
using Redux;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace redux.test
{
    using static Reducers;
    using static Middlewares;

    public class StoreTest
  {
    [Fact]
    public void Fact1()
    {
      var a = new Slice("a");
      var b = new Slice("b");
      
      var (countMiddleware, getCount) = TestMiddleware.CountMiddleware(a.ActionTypes.Rename, b.ActionTypes.Rename);

      var reducers = new Dictionary<string, Reducer>
      {
        [a.StoreKey] = a.Reducer,
        [b.StoreKey] = b.Reducer,
      };

      var store = Redux.Store.CreateStore(
          reducers.Combine(),
          new { },
          ApplyMiddleware(countMiddleware)
      );

      Dispatch dispatch = store.dispatch;
      // 
      var aState = a.Selector(store.getState());
      aState.Should().NotBeNull();
      aState.Name.Should().Be(a.DefaultState.Name);


      dispatch(a.Rename("tom"));
      (a.Selector(store.getState())).Name.Should().Be("tom");

      (b.Selector(store.getState())).Name.Should().Be("?");

      getCount().Should().Be(1);
    }
  }
}
