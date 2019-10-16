using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ValueOf;

namespace redux.test
{
    // see: https://dotnetfiddle.net/Ec7IKN
    // see: https://github.com/JamesNK/Newtonsoft.Json/issues/1230
    public class ValueTupleContractResolver : DefaultContractResolver
    {
        Stack<Queue<string>> names = new Stack<Queue<String>>();

        protected override JsonContract CreateContract(Type objectType)
        {
            var jc = base.CreateContract(objectType);
            var tena = objectType.GetCustomAttributes().OfType<TupleElementNamesAttribute>().SingleOrDefault();
            if (tena != null)
            {
                names.Push(new Queue<string>(tena.TransformNames));
            }
            return jc;
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.Name == "Value" && member.DeclaringType.IsConstructedGenericType && member.DeclaringType.GetGenericTypeDefinition() == typeof(ValueOf<,>))
            {
                property.Writable = true;
            }
            if (names.Count > 0)
            {
                var q = names.Peek();
                var name = q.Dequeue();
                if (name != null)
                {
                    property.PropertyName = name;

                }
                if (q.Count == 0)
                    names.Pop();
            }
            return property;
        }
    }
}