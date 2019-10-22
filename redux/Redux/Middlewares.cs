using System;
using System.Linq;

namespace Redux
{
    // using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;
    using static Redux.Utils;
    public class Middlewares
    {

        /// <summary>
        /// Signature helper
        public static Enhance ApplyMiddleware(params Middleware[] middleware)
        {
            return store =>
            {
                // if ((middleware?.Length ?? 0) == 0) return store;                
                var (dispatch, getState, subscribe) = store;
                return (
                    dispatch: Compose(middleware.Select(m => m((dispatch, getState))(store.dispatch))),
                    getState,
                    subscribe
                );
            };
        }
        /// </summary>    
        public static Middleware CreateMiddleware(Middleware f)
        {
            return f;
        }

    }
}
