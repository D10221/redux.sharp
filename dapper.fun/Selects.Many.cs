using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dapper.fun
{
    using static dapper.fun.Transforms;
    public partial class Selects
    {
        /**
         *  // Sample 
            select * from Customers where CustomerId = @id
            select * from Orders    where CustomerId = @id
            select * from Returns   where CustomerId = @id
         */
        public static Select<object, SqlMapper.GridReader> SelectMany(QueryString query, params Type[] types)
        {
            return (con, tran) => (p) => SqlMapper.QueryMultipleAsync(
                    cnn: con,
                    sql: query,
                    param: AutoName(p),
                    transaction: tran,
                    commandTimeout: query.CommandTimeout,
                    commandType: query.CommandType);
        }
      
        static Func<Task<SqlMapper.GridReader>, Task<R>> WithReader<R>(Func<SqlMapper.GridReader, R> fun)
        {
            return async results => fun((await results));
        }
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>)> SelectMany<T1, T2>(QueryString query) =>
            ChangeResult(SelectMany(query, typeof(T1), typeof(T2)),
                WithReader(reader => (reader.Read<T1>(), reader.Read<T2>())));
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> SelectMany<T1, T2, T3>(QueryString query) =>
            ChangeResult(SelectMany(query, typeof(T1), typeof(T2), typeof(T3)),
                WithReader(reader => (reader.Read<T1>(), reader.Read<T2>(), reader.Read<T3>())));

    }
}