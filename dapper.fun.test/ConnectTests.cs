using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;

    [TestClass]
    public class ConnectTests
    {        
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
        public async Task TestConnected()
        {
            using (var connection = Database.Connect())
            {
                var select1 = Connected(QuerySingle<int>("select 1"), connection);

                var one = await select1();
                one.Should().Be(1);
                // Auto Named as @param
                var select = Connected(QuerySingle<int, int>("select @param"), connection);

                one = await select(1);
                one.Should().Be(1);

            }
        }

        


    }
}
