using System.Data;

namespace dapper.fun
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public delegate Task<R> Selector<P, R>(P param);
    public delegate Task<R> Selector<R>();

    public delegate Selector<P, R> Select<P, R>(IDbConnection connection, IDbTransaction transaction);
    public delegate Selector<R> Select<R>(IDbConnection connection, IDbTransaction transaction);

    public delegate Select<P, R> SelectFty<P, R>(string query);
    public delegate Select<R> SelectFty<R>(string query);

    public class Operations
    {
        public static Select<P, int> Exec<P>(string query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteAsync(connection, query, param: param, transaction: transaction);
        }
        public static Select<int> Exec(string query)
        {
            return (connection, transaction) => () => SqlMapper.ExecuteAsync(connection, query, param: null, transaction: transaction);
        }
        public static Select<P, R> Scalar<P, R>(string query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteScalarAsync<R>(connection, query, param: param, transaction: transaction);
        }
        public static Select<R> Scalar<R>(string query)
        {
            return (connection, transaction) => () => SqlMapper.ExecuteScalarAsync<R>(connection, query, param: null, transaction: transaction);
        }
        public static Select<IEnumerable<R>> Query<R>(string query)
        {
            return (connection, transaction) => () => SqlMapper.QueryAsync<R>(connection, query, param: null, transaction: transaction);
        }
        public static Select<P, IEnumerable<R>> Query<P, R>(string query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryAsync<R>(connection, query, param: param, transaction: transaction);
        }
    }
}