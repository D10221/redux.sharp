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
    using static dapper.fun.Queries;
    using static dapper.fun.Connects;
    [TestClass]
    public class QueryStringTests
    {

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