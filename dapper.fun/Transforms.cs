namespace dapper.fun
{
    using System;
    using System.Threading.Tasks;

    public delegate T Change<T>(T i);
    public delegate Change<T> ChangeFty<T>(T i);

    public class Transforms
    {
        public static Func<ChangeFty<string>, Func<string, Func<string, Select<R>>>> ChangeQuery<R>(SelectFty<R> select)
        {
            return transform => query => input => select(transform(input)(query));
        }
        public static Func<ChangeFty<string>, Func<string, Func<string, Select<P, R>>>> ChangeQuery<P, R>(SelectFty<P, R> select)
        {
            return transform => query => input => select(transform(input)(query));
        }
        public static Select<O, R> ChangeParam<P, R, O>(Select<P, R> select, Func<O, P> transform)
        {
            return (con, tran) => (value) => select(con, tran)(transform(value));
        }
        public static Select<P, X> ChangeResult<P, R, X>(Select<P, R> select, Func<Task<R>, Task<X>> transform)
        {
            return (con, tran) => (p) => transform(select(con, tran)(p));
        }
    }
}