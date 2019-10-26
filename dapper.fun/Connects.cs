using System.Data;

namespace dapper.fun
{
    using System;

    public class Connects
    {
        public static Func<Select<P, R>, Selector<P, R>> Connect<P, R>(IDbConnection connection, IDbTransaction transaction = null)
        {
            return (select) => select(connection, transaction);
        }
        public static Func<Select<R>, Selector<R>> Connect<R>(IDbConnection connection, IDbTransaction transaction = null)
        {
            return (select) => select(connection, transaction);
        }

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
    }
}