using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;

    [TestClass]
    public class SelectsTests
    {
        class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        [TestMethod]
        public async Task MiniCrudTest()
        {
            Select<int> create = Exec("create table if not exists User( id int primary key , name text not null )");
            Select<User, int> insert = Exec<User>("insert into user ( Name ) values ( @Name )");
            Select<IEnumerable<User>> all = Query<User>("select * from user");
            // Wrong
            Select<int, IEnumerable<User>> find = Query<int, User>("select * from user where user id = @id");
            Select<int> drop = Exec("drop table user");

            IEnumerable<User> users;

            Database.Drop();
            using (var cnx = Database.Connect())
            {
                var r = await create(cnx, null)();
                r = await insert(cnx, null)(new User { Name = "bob" });
                users = await all(cnx, null)();
            }

            users.Should().NotBeNull();
            var user = users.FirstOrDefault();
            user.Should().NotBeNull();
            user.Name.Should().Be("bob");
        }
    }
}
