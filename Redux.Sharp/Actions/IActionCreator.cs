namespace Redux.Actions;
public interface IActionCreator
{
    object Type { get; }
    IAction Create();
}
public interface IActionCreator<TPayload>
{
    object Type { get; }
    IAction Create(TPayload payload);
}