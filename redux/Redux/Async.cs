using System;
using Dispatch = System.Func<object, object>;
using System.Threading.Tasks;

namespace Redux
{
    using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;

    public class Async
    {
        static async Task<T> Await<T>(Task<T> task)
        {
            T r = await task;
            return r;
        }

        public static Middleware Middleware = store => next => action =>
        {
            if (action is IAction) return next(action);

            var task = action as Task<object>;
            if (task != null) return next(Await(task));

            return next(action);
        };
    }
}
