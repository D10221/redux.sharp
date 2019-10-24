using System.Data;

namespace server.Database
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public delegate Task<T> Scalar<T>(object param);

    public delegate Scalar<T> MakeScalar<T>(IDbConnection connection, IDbTransaction transaction);

    public delegate Task<T> MakeScalarQuery<T>(string query, object param);

    public delegate Task<T> MakeScalarQuery<T, P>(string query, P param);
    
    public class Users
    {
        public static object D(IDbConnection connection, IDbTransaction transaction = null)
        {

            Scalar<T> Scalar<T>(string query)
            {
                return (args) => connection.ExecuteScalarAsync<T>(query, param: args, transaction: transaction);
            };

            Func<object, Task<IEnumerable<T>>> Query<T>(string query)
            {
                return (args) =>
                {
                    return connection.QueryAsync<T>(query, param: args, transaction: transaction);
                };
            }

            Func<object, Task<int>> Exec(string query)
            {
                return (args) => connection.ExecuteAsync(query, param: args, transaction: transaction);
            };

            Func<User, Task<int>> update = Exec(@"
                    UPDATE User 
                        SET Name = @Name,
                            Password = @Password,
                            Roles = @Roles
                    where id = @id
                ");


            Func<int, Task<User>> get = async (int id) => (
                await Query<User>(@"select * from User where id = @id")(new { id })
                ).FirstOrDefault();

            var add = Scalar<int>(@"
                    insert into USER (
                        Name, Password, Roles
                    ) VALUES (
                        @Name, @Password, @Roles
                    );
                    SELECT last_insert_rowid();
                ");

            Func<Task<IEnumerable<User>>> all = () => Query<User>("select * from User")(null);

            Func<int, Task<int>> delete = (id) => Exec(@"delete User where id = @id")(new { id });

            Func<Task<int>> count = () => Scalar<int>("select count(*) from User ")(null);

            return (
                add: add,
                get: get,
                all: all,
                update: update,
                delete: delete,
                count: count
            );
        }        
    }
}