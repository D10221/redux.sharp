using System;
using System.Threading.Tasks;

namespace Redux
{
    public delegate object Thunk(Dispatch dispatch, GetState getState);
    public delegate Task<object> AsyncThunk(Dispatch dispatch, GetState getState);
   
    public class Thunks
    {
        public static Middleware Middleware = store => next => action =>
           {
               switch(action){
                   case Thunk thunk : return next(thunk(store.dispatch, store.getState));
                   case AsyncThunk thunk: return next(thunk(store.dispatch, store.getState));
                   default: return next(action);
               }               
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
        public static AsyncThunk AsyncThunk(AsyncThunk thunk)
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
        public static Func<T, AsyncThunk> AsyncThunkFty<T>(Func<T, AsyncThunk> f)
        {
            return f;
        }
    }
}
