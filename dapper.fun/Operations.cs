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

    public delegate Select<P, R> SelectFty<P, R>(QueryString query);
    public delegate Select<R> SelectFty<R>(QueryString query);

    public class Operations
    {
        public static Select<P, int> Exec<P>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteAsync(
                connection,
                query,
                param: param,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<int> Exec(QueryString query)
        {
            return (connection, transaction) => () => SqlMapper.ExecuteAsync(
                connection,
                query,
                param: null,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<P, R> Scalar<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteScalarAsync<R>(
                connection,
                query,
                param: param,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<R> Scalar<R>(QueryString query)
        {
            return (connection, transaction) => () => SqlMapper.ExecuteScalarAsync<R>(
                connection,
                query,
                param: null,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<IEnumerable<R>> Query<R>(QueryString query)
        {
            return (connection, transaction) => () => SqlMapper.QueryAsync<R>(
                connection,
                query,
                param: null,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<P, IEnumerable<R>> Query<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryAsync<R>(
                connection, query,
                param: param,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
    }
}