using System;

namespace Redux
{
    public class Actions
    {
        class Action : IAction
        {
            public object Type { get; }
            public object Payload { get; }

            public Action(object type, object payload)
            {
                Type = type;
                Payload = payload;
            }
        }

        public static IAction CreateAction(object type, object payload = null)
        {
            return new Action(type, payload);
        }
        public static Func<T, IAction> CreateActionCreator<T>(object type)
        {
            return (payload) => CreateAction(type, payload);
        }
        public static Func<IAction> CreateActionCreator(object type)
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