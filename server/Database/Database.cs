using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace server.Database
{
    using static System.IO.Path;
    using static System.IO.File;
    using static System.IO.Directory;

    static class Context
    {
        static string DBPath => Combine(Base, "database.db");

        static string Base => Combine(GetCurrentDirectory(), "Database");

        public static IDbConnection Connect()
        {
            return new SQLiteConnection("Data Source=" + DBPath);
        }

        public static async Task Setup()
        {
            using (var cnx = Connect())
            {
                await ExecuteFile(cnx)(Combine(Base, "User.Create.sqlite"));
                await ExecuteFile(cnx)(Combine(Base, "Contact.Create.sqlite"));

                var users = Users.Data();
                
                // if (!(await users.all()).Any())
                // {
                //     await users.add(new User
                //     {
                //         Name = "admin",
                //         Password = "password",
                //         Roles = "admin"
                //     });
                // }
            }
        }

        private static Func<string, Task> ExecuteFile(IDbConnection cnx)
        {
            return async fileName =>
            {
                var sql = await ReadAllTextAsync(fileName);
                await cnx.ExecuteAsync(sql);
            };
        }
    }
}