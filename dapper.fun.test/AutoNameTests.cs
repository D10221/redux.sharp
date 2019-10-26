using System;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Operations;

    [TestClass]
    public class AutoNameTests
    {
        [TestMethod]
        public void DoesNotAutoName()
        {
            AutoName(new { x = 1 }).Should().BeEquivalentTo(new { x = 1 });
        }
        [TestMethod]
        public void AutonamesNumbers()
        {
            AutoName(1).Should().BeEquivalentTo(new { param = 1 });

            AutoName((System.Numerics.BigInteger)1).Should()
                .BeEquivalentTo(new { param = (System.Numerics.BigInteger)1 });

            AutoName((long)1).Should().BeEquivalentTo(new { param = 1 });

            AutoName(1L).Should().BeEquivalentTo(new { param = 1 });

            AutoName((byte)1).Should().BeEquivalentTo(new { param = (byte)1 });

            AutoName(1D).Should().BeEquivalentTo(new { param = 1D });

            var bytes = new byte[0];
            AutoName( bytes).Should().BeEquivalentTo(new{
                param = bytes
            }, "Bytes");
        }
        [TestMethod]
        public void AutonamesStrings()
        {
            AutoName("1").Should().BeEquivalentTo(new { param = "1" });
        }
        [TestMethod]
        public void AutonamesDates()
        {
            var now = DateTime.Now;
            AutoName(now).Should().BeEquivalentTo(new { param = now });
        }
        enum MyEnum
        {
            None,
            One
        }
        [TestMethod]
        public void AutonamesEnums()
        {
            AutoName(MyEnum.One).Should().BeEquivalentTo(new { param = 1 });
        }
        [TestMethod]
        public void AutonamesTimeSpan()
        {
            AutoName(TimeSpan.FromHours(1)).Should().BeEquivalentTo(new { param = TimeSpan.FromHours(1) });
        }
    }
}
