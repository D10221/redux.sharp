using System;
using System.Collections.Generic;
using Subscribe = System.Func<System.Action, System.IDisposable>;
using Dispatch = System.Func<object, object>;
using System.Linq;

namespace Redux
{
  using Reducer = Func<object, object, object>;
  using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;
  using GetState = Func<object>;
  public static class Store
  {
    public static Func<T, T> Compose<T>(IEnumerable<Func<T, T>> funcs)
    {
      return funcs.Aggregate((a, b) => x => a(b(x)));
    }
    public static Func<(Dispatch dispatch, Func<Object> getState), Dispatch> ApplyMiddleware(
                params Middleware[] middleware)
    {
      return store =>
      {
        var dispatches = middleware.Select(m => m(store)(store.dispatch));
        return Compose(dispatches);
      };
    }
    public static (Dispatch dispatch, GetState getState, Subscribe subscribe) CreateStore(
        Reducer reducer,
        object initialState,
        params Middleware[] middleware
    )
    {
      var (dispatch, getState, subscribe) = CreateStore(reducer, initialState);
      if ((middleware?.Length ?? 0) == 0) return (dispatch, getState, subscribe);
      var d = ApplyMiddleware(middleware)((dispatch, getState));
      return (dispatch: d, getState, subscribe);
    }
    static readonly IAction INIT = Actions.CreateAction("@@INIT");
    public static (Dispatch dispatch, GetState getState, Subscribe subscribe) CreateStore(
        Reducer reducer,
        object initialState
    )
    {
      var state = initialState;

      var subscribers = new List<System.Action>();

      void setState(object value)
      {
        state = value;
        foreach (var subscription in subscribers)
        {
          subscription();
        }
      }

      object _lock = new object();

      object dispatch(object action)
      {
        lock (_lock)
        {
          setState(reducer(state, action));
        }
        return action;
      }

      IDisposable subscribe(Action action)
      {
        subscribers.Add(action);
        return Disposable.Create(() =>
        {
          subscribers.Remove(action);
        });
      }

      object getState()
      {
        return state;
      }

      dispatch(INIT);
      return (dispatch, getState, subscribe);
    }
  }
}