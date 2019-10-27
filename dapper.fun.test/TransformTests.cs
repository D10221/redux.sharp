using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Transforms;
    using static dapper.fun.Queries;
    using static dapper.fun.Connects;
    [TestClass]
    public class TransformTests
    {
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
        class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Roles { get; set; }
        }
        [TestMethod]
        public async Task TransformsParameters()
        {
            Database.Drop();
            using (var connection = Database.Connect())
            {
                Select<int, User> Find = ChangeParam(
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
    }
}