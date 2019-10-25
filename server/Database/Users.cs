using System;
using System.Collections.Generic;
using System.Data;
using dapper.fun;

namespace server.Database
{
    using static dapper.fun.Operations;

    public class Users
    {
        public static object Data()
        {
            var update = Exec<User>(@"
                    UPDATE User 
                        SET Name = @Name,
                            Password = @Password,
                            Roles = @Roles
                    where id = @id
                ");
            var find = Query<int, User>(@"select * from User where id = @ID");
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