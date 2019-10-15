using System;
using Dispatch = System.Func<object, object>;
using System.Threading.Tasks;

namespace Redux
{
    using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;
    using Thunk = Func<Dispatch, Func<object>, object>;
    using AsyncThunkFunc = Func<Dispatch, Func<object>, Task<object>>;
    public class Thunks
    {
        public static Middleware Middleware = store => next => action =>
           {
               if (action is IAction) return next(action);

               var thunk = action as Thunk;
               if (thunk != null)
               {
                   object ret = thunk(store.dispatch, store.getState);
                   return next(thunk(store.dispatch, store.getState));
               }

               return next(action);
           };
        ///<summary>
        ///  Signature helper
        ///</summary>
        public static Thunk Thunk(Thunk thunk)
        {
            return thunk;
        }
        ///<summary>
        ///  Signature helper
        ///</summary>
        public static AsyncThunkFunc AsyncThunk(AsyncThunkFunc thunk)
        {
            return thunk;
        }
        ///<summary>
        ///  Signature helper
        ///</summary>
        public static Func<T, Thunk> ThunkFty<T>(Func<T, Thunk> f)
        {
            return f;
        }
        ///<summary>
        ///  Signature helper
        ///</summary>
        public static Func<T, AsyncThunkFunc> AsyncThunkFty<T>(Func<T, AsyncThunkFunc> f)
        {
            return f;
        }
    }
}
