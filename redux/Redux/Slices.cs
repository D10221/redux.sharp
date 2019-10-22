using System;
// using Dispatch = System.Func<object, object>;

namespace Redux
{
    //using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;
        
    public class Slices
    {
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