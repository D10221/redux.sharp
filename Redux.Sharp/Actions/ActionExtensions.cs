namespace Redux.Actions;
public static class ActionExtensions
{
    public static bool IsType(this IAction action, object actionType) => Equals(action.Type, actionType);
    public static object Dispatch<TState>(this IStore<TState> store, object type, params object[] args) => store.Dispatch(ActionFactory.CreateAction(type, args));
}
