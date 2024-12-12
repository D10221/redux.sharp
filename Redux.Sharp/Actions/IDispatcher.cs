namespace Redux.Actions;
public interface IDispatcher 
{
    object Type { get; }
    object Invoke();
}
public interface IDispatcher<TPayload>
{
    object Type { get; }
    object Invoke(TPayload payload);
}
