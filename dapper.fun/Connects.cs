using System.Data;

namespace dapper.fun
{
    using System;
    using System.Threading.Tasks;

    public class Connects
    {
        public static Func<IDbConnection, Selector<P, R>> Connect<P, R>(Select<P, R> select)
        {
            return (connection) => select(connection, null);
        }
        public static Func<IDbConnection, Selector<R>> Connect<R>(Select<R> select)
        {
            return (connection) => select(connection, null);
        }
        public static Selector<P, R> Connect<P, R>(Select<P, R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return select(connection, transaction);
        }
        public static Selector<R> Connect<R>(Select<R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return select(connection, transaction);
        }
        public static Func<Task<R>> Connected<R>(Select<R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return () => select(connection, transaction)();
        }
        public static Func<P, Task<R>> Connected<P, R>(Select<P, R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (p) => select(connection, transaction)(p);
        }
    }
}