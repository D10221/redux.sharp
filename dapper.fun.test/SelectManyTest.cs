using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;
    [TestClass]
    public class SelectManyTest
    {
        class A
        {
            public int Value { get; set; }
        }
        class B
        {
            public int Value { get; set; }
        }
        class C
        {
            public string StringValue { get; set; }
        }

        [TestMethod]
        public async Task Selects2()
        {
            using (var con = Database.Connect())
            {
                var select = Connect(NoParams(SelectMany<A, B>(@"
                    select 1 as Value;
                    select 2 as Value
                ")), con);
                var (a, b) = await select();
                
                a.Should().BeEquivalentTo(new A { Value = 1 });
                b.Should().BeEquivalentTo(new B { Value = 2 });
            }
        }
        [TestMethod]
        public async Task Selects3()
        {
            using (var con = Database.Connect())
            {
                var select = Connect(NoParams(SelectMany<A, B, C>(@"
                    select 1 as Value;
                    select 2 as Value;
                    select 'x' as StringValue
                ")), con);
                var (a, b, c) = await select();

                a.Should().BeEquivalentTo(new A { Value = 1 });
                b.Should().BeEquivalentTo(new B { Value = 2 });
                c.Should().BeEquivalentTo(new C { StringValue = "x" });
            }
        }
    }
}