using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fty.test
{
    using static dapper.fty.Operations;
    using static dapper.fty.Transforms;
    using static dapper.fty.Queries;
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Test1()
        {
            Select<int> create = Exec(User.Scripts.Create);
            Select<User, int> insert = Exec<User>(User.Scripts.Insert);
            Select<IEnumerable<User>> all = Query<User>(User.Scripts.All);
            // Wrong
            Select<int, IEnumerable<User>> find = Query<int, User>(User.Scripts.Find);
            Select<int> drop = Exec(User.Scripts.Drop);
            Func<string, Select<IEnumerable<User>>> where = Change(Query<User>)(WithWhere)(User.Scripts.All);

            IEnumerable<User> users;

            Database.Drop();
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

        [TestMethod]
        public async Task TestWhereNoParams()
        {
            var where = Change(Query<object>)(WithWhere)(
                "WITH x AS (values(0,1,2,3,4)) select * from x"
            );
            Database.Drop();
            IEnumerable<object> result;
            using (var cnx = Database.Connect())
            {
                var withWhere = where(" column2 = 1 ");
                var execWhere = withWhere(cnx, null);
                result = await execWhere(); //no params
            }
            ((int)(((dynamic)result.FirstOrDefault()).column2)).Should().Be(1);
        }

        [TestMethod]
        public async Task TestWhereParams()
        {
            var where = Change(Query<object, object>)(WithWhere)(
                "WITH x AS (values(0,1,2,3,4)) select * from x"
            );

            IEnumerable<object> result;

            Database.Drop();
            using (var cnx = Database.Connect())
            {
                result = await where(" column2 = @value ")(cnx, null)(new { value = 1 });
            }

           ((int)(((dynamic)result.FirstOrDefault()).column2)).Should().Be(1);
        }
    }

}
