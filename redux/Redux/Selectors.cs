using System;
using System.Collections.Generic;

namespace Redux {
    public class Selectors {
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
    }
}