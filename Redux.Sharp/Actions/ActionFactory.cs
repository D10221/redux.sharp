namespace Redux.Actions;
public static class ActionFactory
{
    public static IActionCreator Create(object type) => new ActionCreator(type);
    public static IActionCreator<TPayload> Create<TPayload>(object type) => new ActionCreator<TPayload>(type);
    public static IDispatcher Create(object type, Dispatcher dispatcher) => new ActionDispatcher(type, dispatcher);
    public static IDispatcher<TPayload> Create<TPayload>(object type, Dispatcher dispatcher) => new ActionDispatcher<TPayload>(type, dispatcher);
    public static IAction CreateAction(object type, params object[] payload) => new Action(type, payload);    
}