namespace Redux.Actions;
class ActionDispatcher(object type, Dispatcher dispatcher) : ActionCreator(type), IDispatcher
{
    public object Invoke() => dispatcher(Create());
}
class ActionDispatcher<TPayload>(object type, Dispatcher dispatcher) : ActionCreator<TPayload>(type), IDispatcher<TPayload>
{
    public object Invoke(TPayload payload) => dispatcher(Create(payload));
}