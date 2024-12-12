namespace Redux.Actions;
class ActionCreator<TPayload>(object type) : IActionCreator<TPayload>
{
    public object Type { get; } = type;
    public IAction Create(TPayload payload) => ActionFactory.CreateAction(Type, payload);
}
class ActionCreator(object type) : IActionCreator
{
    public object Type => type;
    public IAction Create() => ActionFactory.CreateAction(Type);
}