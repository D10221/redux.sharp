namespace Redux;
using System.Collections.Generic;
public static class Extensions
{
    public static Func<T, T> Compose<T>(this IEnumerable<Func<T, T>> funcs) => funcs.Aggregate((a, b) => x => a(b(x)));
    public static Reducer<TState> Combine<TState>(this IEnumerable<Reducer<TState>> reducers) => reducers.Aggregate((r1, r2) => (s, a) => r1(r2(s, a), a));
    public static Func<Dispatcher, Dispatcher> ApplyMiddleware<TState>(this IEnumerable<Middleware<TState>> middleware, IStore<TState> store)
    {
        if (!middleware.Any()) return (d) => d;
        return middleware.Select(m => m(store)).Compose();
    }
    public static bool Implements(this Type type, Type expectedBaseType) => expectedBaseType.IsAssignableFrom(type) && type != expectedBaseType;
}