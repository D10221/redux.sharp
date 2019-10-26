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
    public class Selects
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
                param: AutoName(param),
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
    }
}