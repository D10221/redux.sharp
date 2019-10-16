using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ValueOf;

namespace redux.test
{
  public class IgnoreAttribute : Attribute { }

  // see: https://dotnetfiddle.net/Ec7IKN
  // see: https://github.com/JamesNK/Newtonsoft.Json/issues/1230
  //This isn't strictly necessary, it's used to remove the Value property when serializing the ValueOf	
  class ValueOfJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      if (objectType.BaseType.IsGenericType)
        if (objectType.BaseType.GetGenericTypeDefinition() == typeof(ValueOf<,>))
          return true;
      return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var jt = Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
      var valueType = objectType.BaseType.GetGenericArguments()[0];
      var value = jt.ToObject(valueType, serializer);
      var from = objectType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
          .FirstOrDefault(m => m.Name == "From" && m.GetCustomAttribute<IgnoreAttribute>() == null);
      return from.Invoke(null, new[] { value });
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      var innerValue = ((dynamic)value).Value;
      serializer.Serialize(writer, innerValue);

    }
  }
}