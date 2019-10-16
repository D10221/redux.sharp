using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Redux
{
  using Reducer = Func<object, object, object>;
  public static class Reducers
  {   
    /// <summary>
    /// Signatunre helper, untyped, & secures (state ?? initialState)
    /// </summary>    
    public static Reducer CreateReducer(Reducer reducer, object initialState)
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
    public static Reducer CreateReducer<T>(T initialState, Func<T, object, T> reducer)
    {
      return (object state, object action) =>
      {
        state = state ?? initialState;
        return reducer(state is T ? (T)state : default, action);
      };
    }     
    public static Reducer CombineReducer(IDictionary<string, Reducer> reducers)
    {
      return (_state, action) =>
      {
        var state = ToDictionary(_state);

        foreach (var key in reducers.Keys)
        {
          if (!reducers.TryGetValue(key, out var reducer))
          {
            Debug.WriteLine($"CombineReducer: reducer '{key}' Not Found");
          }
          if (!state.TryGetValue(key, out var slice))
          {
            Debug.WriteLine($"CombineReducer: state key '{key}' Not Found");
          }
          state[key] = reducer?.Invoke(slice, action) ?? state;
        }
        return state;
      };
    }
    public static Reducer Combine(this IDictionary<string, Reducer> reducers)
    {
      return CombineReducer(reducers);
    }
    private static IDictionary<string, object> ToDictionary(object state)
    {
      if (state is IDictionary<string, object>) return (IDictionary<string, object>)state;
      return state.GetType().GetProperties().ToDictionary(prop => prop.Name, prop => prop.GetValue(state));
    }
  }
}