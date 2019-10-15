using System;

namespace Redux
{
  public class Action : IAction
  {
    public object Type { get; private set; }
    public object Payload { get; private set; }

    public static Action CreateAction(object type, object payload = null)
    {
      return new Action
      {
        Type = type,
        Payload = payload
      };
    }
    public static Func<T, Action> CreateActionCreator<T>(object type)
    {
      return (payload) => CreateAction(type, payload);
    }
    public static Func<Action> CreateActionCreator(object type)
    {
      return () => CreateAction(type);
    }
    public static bool IsActionType(object action, object actionType)
    {
      return object.Equals((action as IAction)?.Type, actionType);
    }
    public static Func<object, bool> IsActionType(object action)
    {
      return actionType => IsActionType(action, actionType);
    }
    public static object GetActionType(object action)
    {
      return (action as IAction)?.Type;
    }
  }
}