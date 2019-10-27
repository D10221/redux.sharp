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