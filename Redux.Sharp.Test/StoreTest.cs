using FluentAssertions;

namespace Redux.Test;

[TestClass]
public class StoreTests
{
    /// <summary>
    /// Can Create Store
    /// </summary>
    [TestMethod]
    public void Test1() => Test(new Store<int>((state, action) => action switch
        {
            "inc" => state + 1,
            "dec" => state - 1,
            _ => state
        }));
    int Reduce(int state, object action)
    {
        return action switch
        {
            "inc" => state + 1,
            "dec" => state - 1,
            _ => state
        };
    }
    /// <summary>
    /// Can Create Store with Reducer
    /// </summary>
    [TestMethod]
    public void Test2() => Test(new Store<int>(Reduce));
    /// <summary>
    /// Can Run Middleware
    /// </summary>
    [TestMethod]
    public void MiddlewareTest1()
    {
        var called = false;
        Middleware<int> middleware = store =>
        {
            return next => action =>
                    {
                        called = true;
                        return next(action);
                    };
        };
        var store = Store.From(Reduce, middleware: middleware);
        Test(store);
        Assert.IsTrue(called);
    }
    
    [TestMethod]
    public void MiddlewareTest2()
    {
        var called = false;
        var store = new Store<int>(Reduce, middleware: store => next => action =>
        {
            called = true;
            return next(action);
        });
        Test(store);
        Assert.IsTrue(called);
    }

    [TestMethod]
    public void MiddlewareTest3()
    {
        var called1 = false;
        var called2 = false;
        Middleware<int> middleware1 = store => next => action =>
                {
                    called1 = true;
                    return next(action);
                };
        Middleware<int> middleware2 = store => next => action =>
                {
                    called2 = true;
                    return next(action);
                };
        var store = new Store<int>(Reduce, 0, middleware1, middleware2);        
        Test(store);
        Assert.IsTrue(called1);
        Assert.IsTrue(called2);
    }

    public static void Test(IStore<int> store)
    {
        var changed = false;
        store.State.Should().Be(0);
        using (store.Subscribe((s) => changed = true))
        {
            store.Dispatch("inc");
            store.State.Should().Be(1);
        }
        changed.Should().BeTrue();
        changed = false;
        store.Dispatch("dec");
        store.State.Should().Be(0);
        changed.Should().BeFalse();
    }
}
