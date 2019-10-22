using System;
using System.Collections.Generic;

namespace Redux
{
    
    public static class Store
    {
        public static (Dispatch dispatch, GetState getState, Subscribe subscribe) CreateStore(
            Reducer reducer,
            object initialState,
            Enhance enhance
        )
        {
            var store = CreateStore(reducer, initialState);                       
            return enhance(store);
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