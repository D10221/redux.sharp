using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Operations;
    using static dapper.fun.Transforms;
    using static dapper.fun.Queries;
    using static dapper.fun.Connects;

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
            Func<string, Select<IEnumerable<User>>> where = ChangeQuery(Query<User>)(WithWhere)(User.Scripts.All);

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
            var where = ChangeQuery(Query<object>)(WithWhere)(
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
            var where = ChangeQuery(Query<object, object>)(WithWhere)(
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
        class Values
        {
            public int Column1 { get; set; }
            public int Column2 { get; set; }
            public int Column3 { get; set; }
        }
        [TestMethod]
        public async Task TestConnect()
        {
            Values test(Values x)
            {
                x.Should().NotBeNull();
                x.Should().BeOfType<Values>();
                x.Column1.Should().Be(1);
                x.Column2.Should().Be(2);
                x.Column3.Should().Be(3);
                return x;
            }

            Database.Drop();
            using (var connection = Database.Connect())
            {
                var query = "WITH x AS (values(1,2,3)) select * from x";
                test(await Connect(QuerySingle<Values>(query))(connection)());
                // connect using connection , dont pass null 
                test(await Connect(QuerySingle<Values>(query), connection)());
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // connect using connection , and transaction
                    test(await Connect(QuerySingle<Values>(query), connection, transaction)());
                }
                // without connect: transaction must be null if not used
                test(await QuerySingle<Values>(query)(connection, null)());
            }
        }

        [TestMethod]
        public async Task TransformsParameters()
        {
            Database.Drop();
            using (var connection = Database.Connect())
            {
                Select<int, User> Find = ChangeParameters(
                                QuerySingle<object, User>(@"
                -- SQLite
                WITH x AS (values(1,'bob','password', 'admin')) 
                select 
                    x.Column1 as ID,
                    x.COlumn2 as Name,
                    x.Column3 as Password,
                    x.COlumn4 as Roles
                from x
                WHERE id = @ID
                "),
                  transform: (int id) => new { ID = id }
                );
                var find = Connect(Find, connection);
                var found = await find(1);
                var subject = found.Should();
                subject.NotBeNull();
                subject.BeOfType<User>();
                found.Name.Should().Be("bob");
            }

        }

        [TestMethod]
        public async Task SqlQueryOptions()
        {
            // implicit conversions 
            using (var connection = Database.Connect())
            {
                var text = "select 1";
                var x = await Connect(QuerySingle<int>((text, commandTimeout: 30, commandType: CommandType.Text)))(connection)();
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>((text, commandTimeout: 30)))(connection)();
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>((text, /*commandTimeout:*/ 30)))(connection)();
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>((text, commandType: CommandType.Text)))(connection)();                
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>((text, CommandType.Text)))(connection)();
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>(new QueryString(text: text, commandTimeout: 30, commandType: CommandType.Text)))(connection)();
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>(new QueryString(text: text)))(connection)();
                x.Should().Be(1);
                x = await Connect(QuerySingle<int>(text))(connection)();
                x.Should().Be(1);
            }
        }
    }

}
