using Dapper;
using System;
using System.Collections.Generic;

namespace dapper.fun
{
    ///<summary>
    /// They All select something 
    /// - TODO: Dapper features:
    /// - Buffered
    /// - TFirst/ TSecond, etc (Multi Mapping)
    /// - Multiple Results 
    /// - Store Procedures options
    ///</summary>
    public partial class Selects
    {
        public static Select<P, int> Exec<P>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteAsync(
                connection,
                query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<int> Exec(QueryString query) => NoParams(Exec<object>(query));

        public static Select<P, R> Scalar<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteScalarAsync<R>(
                connection,
                query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<R> Scalar<R>(QueryString query) => NoParams(Scalar<object, R>(query));
        public static Select<P, IEnumerable<R>> Query<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryAsync<R>(
                connection, query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<IEnumerable<R>> Query<R>(QueryString query) => NoParams(Query<object, R>(query));
        public static Select<P, R> QueryFirst<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryFirstAsync<R>(
                connection, query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<R> QueryFirst<R>(QueryString query) => NoParams(QueryFirst<object, R>(query));
        public static Select<P, R> QueryFirstOrDefault<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryFirstOrDefaultAsync<R>(
                connection, query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<R> QueryFirstOrDefault<R>(QueryString query) => NoParams(QueryFirstOrDefault<object, R>(query));
        public static Select<P, R> QuerySingle<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.QuerySingleAsync<R>(
                connection, query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<R> QuerySingle<R>(QueryString query) => NoParams(QuerySingle<object, R>(query));
        public static Select<P, R> QuerySingleOrDefault<P, R>(QueryString query)
        {
            return (connection, transaction) => (param) => SqlMapper.QuerySingleOrDefaultAsync<R>(
                connection, query,
                param: AutoName(param),
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType
                );
        }
        public static Select<R> QuerySingleOrDefault<R>(QueryString query) => NoParams(QuerySingleOrDefault<object, R>(query));
        public static object AutoName<P>(P param)
        {
            switch (typeof(P))
            {
                case Type p when p.IsPrimitive: return new { param };
                case Type p when p.IsValueType: return new { param };
                case Type p when p == typeof(string): return new { param };
                case Type p when p == typeof(byte[]): return new { param };
                default: return param;
            }
        }
        public static Select<R> NoParams<P, R>(Select<P, R> select)
        {
            return (connection, transaction) => () => select(connection, transaction)(default(P));
        }
    }
}