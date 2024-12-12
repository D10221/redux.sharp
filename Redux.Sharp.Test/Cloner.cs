namespace Redux.Test;

static class Cloner
{
  public static T Clone<T>(this T source) => System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize(source));
  public static T Clone<T>(this T source, Action<T> action)
  {
    var clone = source.Clone();
    action(clone);
    return clone;
  }
}
