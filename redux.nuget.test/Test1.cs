using Redux;
using FluentAssertions;
using System;
using Xunit;

namespace redux.nuget.test
{
    using static Redux.Store;
    public class Test1
    {
        [Fact]
        public void TestMethod1()
        {
            var store = CreateStore(
                (state, action) => state,
                new { x = "x" }
            );
            dynamic state = store.getState();
            ((string)state.x).Should().Be("x");
        }
    }
}
