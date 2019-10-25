using System.Data;

namespace dapper.fty
{
    using Dapper;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public delegate Task<R> Selector<P,R>(P param);
    public delegate Selector<P,R> Select<P,R>(IDbConnection connection, IDbTransaction transaction);
    public delegate Select<P,R> CreateSelect<P,R>(string query);
    
    public delegate Task<R> Selector<R>();
    public delegate Selector<R> Select<R>(IDbConnection connection, IDbTransaction transaction);
    public delegate Select<R> CreateSelect<R>(string query);

    public class Operations
    {
        public static Select<P,int> Exec<P>(string query)
        {
            return (connection, transaction) => (param) => connection.ExecuteAsync(query, param: param, transaction: transaction);
        }
               
        public static Select<P,R> Scalar<P,R>(string query)
        {
             return (connection, transaction) => (param) => connection.ExecuteScalarAsync<R>(query, param: param, transaction: transaction);
        }
       
        public static Select<P,IEnumerable<T>> Query<P,T>(string query)
        {
            return (connection, transaction) => (param) => connection.QueryAsync<T>(query, param: param, transaction: transaction);
        }
        // ..
         public static Select<int> Exec(string query)
        {
            return (connection, transaction) => () => connection.ExecuteAsync(query, param: null, transaction: transaction);
        }
               
        public static Select<R> Scalar<R>(string query)
        {
             return (connection, transaction) => () => connection.ExecuteScalarAsync<R>(query, param: null, transaction: transaction);
        }
       
        public static Select<IEnumerable<T>> Query<T>(string query)
        {
            return (connection, transaction) => () => connection.QueryAsync<T>(query, param: null, transaction: transaction);
        }

    }
}