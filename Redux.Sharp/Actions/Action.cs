namespace Redux.Actions;
class Action(object type, params object[] payload) : IAction
{
    public object Type { get; } = type;
    public object[] Payload { get; } = payload;
}