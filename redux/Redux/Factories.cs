using System;
using System.Collections.Generic;
using Dispatch = System.Func<object, object>;
namespace Redux
{
  using Reducer = Func<object, object, object>;
  using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;
  /// <summary>
  /// Type creation Helpers
  /// </summary>
  public static class Factories
  {
    /// <summary>
    ///  Inline Factory helper
    /// </summary>    
    public static Func<A, T> Fty<T, A>(Func<A, T> f) { return a => f(a); }

    /// <summary>
    /// Signatunre helper, untyped, & secures (state ?? initialState)
    /// </summary>    
    public static Func<object, object, object> CreateReducer(Func<object, object, object> reducer, object initialState)
    {
      return (state, action) =>
      {
        state = state ?? initialState;
        return reducer(state, action);
      };
    }
    /// <summary>
    /// Typed signature helper & helps cast to T & secures (state ?? initialState)
    /// </summary>    
    public static Func<object, object, object> CreateReducer<T>(T initialState, Func<T, object, T> reducer)
    {
      return (object state, object action) =>
      {
        state = state ?? initialState;
        return reducer(state is T ? (T)state : default, action);
      };
    }
    /// <summary>
    /// Signature helper
    /// </summary>    
    public static Func<object, T> CreateSelector<T>(Func<object, T> selector)
    {
      return selector;
    }
    /// <summary>
    /// Signature helper
    /// </summary>    
    public static Func<object, T> CreateGenericSelector<T>(string storeKey, T defaultState = default)
    {
      return state =>
      {
        switch (state)
        {
          case null: return defaultState;
          case IDictionary<string, object> dict: return dict.TryGetValue(storeKey, out var result) ? (T)result : default;
          default: return state is T ? (T)state : default;
        }
      };
    }
    /// <summary>
    /// Signature helper
    /// </summary>    
    public static Middleware CreateMiddleware(Middleware f)
    {
      return f;
    }
    /// <summary>
    /// Signature/Struct/Tuple/Record helper
    /// Tries to reduce the need to reference the state's type many times
    /// </summary>    
    public static Func<(
            string storeKey,
            TActionTypes actionTypes,
            TState initialState,
            TActions actions,
            Func<object, TState> selector,
            Reducer reducer,
            Middleware middleware
      )> CreateSlice<TState, TActions, TActionTypes>(
        Func<(string storeKey, TActionTypes actionTypes, TState initialState, TActions actions, Func<object, TState> selector, Reducer reducer, Middleware middleware)>
        f
      )
    {
      return f;
    }
  }
}