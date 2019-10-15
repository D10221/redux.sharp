using Redux;
using System;
using System.Linq;
using Dispatch = System.Func<object, object>;

namespace redux.test
{
  using Middleware = Func<(Dispatch dispatch, Func<object> getState), Func<Dispatch, Dispatch>>;
  using static Redux.Actions;
  class TestMiddleware
  {
    /// <summary>
    /// Counts call to Middleware from Actions of types included in params
    /// </summary>
    /// <param name="actionTypes"></param>
    /// <returns></returns>
    public static (Middleware middleware, Func<int> getCount) CountMiddleware(params object[] actionTypes)
    {
      var count = 0;
      Middleware middleware = _ => next => action =>
      {
        if (actionTypes.Any(IsActionType(action))) count++;
        return next(action);
      };
      return (middleware, getCount: () => count);
    }
  }
}