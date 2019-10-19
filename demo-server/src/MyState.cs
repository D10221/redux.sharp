using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp
{
    class MyState
    {
        static IDictionary<string, object> state = new Dictionary<string, object>{
            {"root", Root()},
            {"user", User()},
            {"home", new {
                name = "home"
            }}
        };
        public static IDictionary<string, object> GetState()
        {
            return state;
        }
        static object Root()
        {
            return new
            {
                x = "x"
            };
        }
        public static object User()
        {
            return new { Name = "Bob" };
        }
        public static string Serialize(object o, bool encoded = false)
        {
            if (encoded)
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o)));
            return JsonConvert.SerializeObject(o);
        }
    }

}
