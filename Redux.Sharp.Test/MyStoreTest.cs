using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;

namespace Redux.Test;

[TestClass]
public partial class MyStoreTest
{
  [TestMethod]
  public void Test1()
  {
    var store = new MyStore(new() { Sub1 = new() { Value = 0 }, Sub2 = new() { Value = "" } });
    var count = 0;
    store.Use(s => next => action =>
    {
      count++;
      return next(action);
    });
    count.Should().Be(0);

    var changed = false;
    store.Subscribe((_) => changed = true);

    store.Sub1.Increment();
    store.Sub1.State.Value.Should().Be(1);
    changed.Should().BeTrue();

    changed = false;
    store.Sub1.Decrement();
    store.Sub1.State.Value.Should().Be(0);
    changed.Should().BeTrue();

    changed = false;
    store.Sub2.Add.Invoke("a");
    store.Sub2.State.Value.Should().Be("a");
    changed.Should().BeTrue();

    changed = false;
    store.Sub2.Replace.Invoke("a");
    store.Sub2.State.Value.Should().Be("");
    changed.Should().BeTrue();

    changed = false;
    store.Sub2.CopySub1.Invoke();
    store.Sub2.State.Value.Should().Be("0");
    changed.Should().BeTrue();
  }
}
