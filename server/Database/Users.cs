using System;
using System.Collections.Generic;
using System.Data;
using dapper.fun;

namespace server.Database
{
    using static dapper.fun.Selects;
    using static dapper.fun.Queries;
    using static dapper.fun.Transforms;

    public class Users
    {
        public static object Data()
        {
            Select<int> create = Exec(User.Scripts.Create);
            Select<User, int> update = Exec<User>(User.Scripts.Update);

            Select<int, User> find = ChangeParam(
                QuerySingle<object, User>(@"select * from User where id = @ID"),
                (int id) => new { ID = id }
            );

            var add = Scalar<User, int>(@"
                    insert into USER (
                        Name, Password, Roles
                    ) VALUES (
                        @Name, @Password, @Roles
                    );
                    SELECT last_insert_rowid();
                ");

            var all = Query<User>("select * from user");
            var delete = Exec(@"delete User where id = @id");
            var count = Scalar<int>("select count(*) from User ");

            return (
                add: add,
                all: all,
                count: count,
                delete: delete,
                find: find,
                update: update
            );
        }
    }
}