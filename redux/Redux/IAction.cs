namespace Redux
{
    public interface IAction
    {
        object Type { get; }
        object Payload { get; }
    }
}