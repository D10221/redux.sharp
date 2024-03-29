﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Redux
{
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
        public static Reducer CombineReducer(IDictionary<string, Reducer> reducers, Func<object, IDictionary<string, object>> toDictionary = null )
        {
            toDictionary = toDictionary ?? Redux.Reducers.ToDictionary;

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
        public static Reducer Combine(this IDictionary<string, Reducer> reducers,  Func<object, IDictionary<string, object>> toDictionary = null )
        {
            return CombineReducer(reducers, toDictionary);
        }
        public static IDictionary<string, object> ToDictionary(object state)
        {
            if (state == null) return new Dictionary<string, object>();
            if (state is IDictionary<string, object>) return (IDictionary<string, object>)state;
            return state.GetType().GetProperties().ToDictionary(prop => prop.Name, prop => prop.GetValue(state));
        }
    }
}