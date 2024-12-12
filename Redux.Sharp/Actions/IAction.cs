namespace Redux.Actions;
public interface IAction
{
    object Type { get; }
    object[] Payload { get; }
}
