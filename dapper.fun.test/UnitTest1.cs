using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Test1()
        {
            Database.Drop();
            using (var con = Database.Connect())
            {
                (await Exec("")(con, null)()).Should().Be(-1);
                (await Exec("create table Empty(id int)")(con, null)()).Should().Be(0);
                (await Exec("insert into Empty(id) values (1)")(con, null)()).Should().Be(1);
                // Execute a Command multiple times
                (await Exec<object>("insert into Empty(id) values (1)")(con, null)(new[] { new { id = 2 }, new { id = 3 } }))
                    .Should().Be(2);

                (await Scalar<int>("select count(*) from Empty")(con, null)()).Should().Be(3);
                (await Scalar<int, int>("select @param")(con, null)(3)).Should().Be(3);
                
                // List Support
                // Dapper allows you to pass in IEnumerable<int> and will automatically parameterize your query.
                (await Query<object,int>(
                    @"select * 
                        from (
                            select 1 as Id 
                            union all 
                            select 2 
                            union all 
                            select 3) 
                            as X where Id in @Ids"
                    )(con, null)(
                        new { Ids = new int[] { 1, 2, 3 } }
                    ))                    
                    .Should()
                    .BeEquivalentTo(new int[] { 1, 2, 3 });
                
                // Literal Support
                (await Query<object, int>(@"
                    select 1 as ID 
                    where Id = {=ID}
                ")
                (con, null)
                (new { ID =1 }))
                .Should()
                .BeEquivalentTo(new []{ 1 });
                
            }
        }
    }
}
