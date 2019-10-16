using System;
using Dispatch = System.Func<object, object>;

namespace Redux
{
    using static Actions;

    public partial class ActionCreators
    {
        public static Func<Dispatch, ValueTuple<Func<T1, object>, Func<T2, object>>> BindActionCreators<T1, T2>(Func<T1, IAction> f1, Func<T2, IAction> f2)
        {
            return (dispatch) => (BindActionCreator(f1)(dispatch), BindActionCreator(f2)(dispatch));
        }
        public static Func<Dispatch, ValueTuple<Func<T1, object>, Func<T2, object>, Func<T3, object>>> BindActionCreators<T1, T2, T3>(Func<T1, IAction> f1, Func<T2, IAction> f2, Func<T3, IAction> f3)
        {
            return (dispatch) => (BindActionCreator(f1)(dispatch), BindActionCreator(f2)(dispatch), BindActionCreator(f3)(dispatch));
        }
        public static Func<Dispatch, ValueTuple<Func<T1, object>, Func<T2, object>, Func<T3, object>, Func<T4, object>>> BindActionCreators<T1, T2, T3, T4>(Func<T1, IAction> f1, Func<T2, IAction> f2, Func<T3, IAction> f3, Func<T4, IAction> f4)
        {
            return (dispatch) => (BindActionCreator(f1)(dispatch), BindActionCreator(f2)(dispatch), BindActionCreator(f3)(dispatch), BindActionCreator(f4)(dispatch));
        }
        public static Func<Dispatch, ValueTuple<Func<T1, object>, Func<T2, object>, Func<T3, object>, Func<T4, object>, Func<T5, object>>> BindActionCreators<T1, T2, T3, T4, T5>(Func<T1, IAction> f1, Func<T2, IAction> f2, Func<T3, IAction> f3, Func<T4, IAction> f4, Func<T5, IAction> f5)
        {
            return (dispatch) => (BindActionCreator(f1)(dispatch), BindActionCreator(f2)(dispatch), BindActionCreator(f3)(dispatch), BindActionCreator(f4)(dispatch), BindActionCreator(f5)(dispatch));
        }
        public static Func<Dispatch, ValueTuple<Func<T1, object>, Func<T2, object>, Func<T3, object>, Func<T4, object>, Func<T5, object>, Func<T6, object>>> BindActionCreators<T1, T2, T3, T4, T5, T6>(Func<T1, IAction> f1, Func<T2, IAction> f2, Func<T3, IAction> f3, Func<T4, IAction> f4, Func<T5, IAction> f5, Func<T6, IAction> f6)
        {
            return (dispatch) => (BindActionCreator(f1)(dispatch), BindActionCreator(f2)(dispatch), BindActionCreator(f3)(dispatch), BindActionCreator(f4)(dispatch), BindActionCreator(f5)(dispatch), BindActionCreator(f6)(dispatch));
        }
    }
}