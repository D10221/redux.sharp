using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fty.test
{
    using static dapper.fty.Operations;
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Test1()
        {
            var create = Exec(User.Scripts.Create);
            var insert = Exec<User>(User.Scripts.Insert);
            var all = Query<User>(User.Scripts.All);

            IEnumerable<User> users;

            using (var cnx = Database.Connect())
            {
                var r = await create(cnx, null)();
                r = await insert(cnx, null)(new User { Name = "bob", Password = "123", Roles = "" });
                users = await all(cnx, null)();
            }
            users.Should().NotBeNull();
            var user = users.FirstOrDefault();
            user.Should().NotBeNull();
            user.Name.Should().Be("bob");
        }
    }
    class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }

        public class Scripts
        {
            public const string Create = @"
    CREATE TABLE IF NOT EXISTS User (
    ID int PRIMARY KEY,
    Name TEXT NOT null,
    Password TEXT NOT Null,
    Roles TEXT Not Null
)
            ";
            public const string Insert = @"
    INSERT INTO USER (
        Name, Password, Roles
    ) VALUES (
        @Name, @Password, @Roles
    )
            ";
            public const string All = @"select * from User";
        }
    }    
}
