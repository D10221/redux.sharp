using System;
using Dispatch = System.Func<object, object>;

namespace Redux
{
    using static Actions;

    public partial class ActionCreators
    {
        public static Func<T, IAction> CreateActionCreator<T>(object type)
        {
            return (payload) => CreateAction(type, payload);
        }
        public static Func<IAction> CreateActionCreator(object type)
        {
            return () => CreateAction(type);
        }

        public static Func<Dispatch, Func<T, object>> BindActionCreator<T>(Func<T, IAction> actionCreator)
        {
            return dispatch => args => dispatch(actionCreator(args));
        }
      
    }
}