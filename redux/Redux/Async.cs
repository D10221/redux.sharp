using System.Threading.Tasks;

namespace Redux
{
    public class Async
    {
        static async Task<T> Await<T>(Task<T> task)
        {
            T r = await task;
            return r;
        }

        public static Middleware Middleware = store => next => action =>
        {            
            var task = action as Task<object>;
            if (task != null)
            {
                return next(Await(task));
            }

            return next(action);
        };
    }
}
