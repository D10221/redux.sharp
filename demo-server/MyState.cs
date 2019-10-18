using Newtonsoft.Json;
using System;
using System.Text;

namespace MyApp
{
    class MyState
    {
        public static object GetState()
        {
            return new { 
                User = User()
            };
        }
        public static object User(){
            return new { Name = "Bob" };
            
        }
        public static string Serialize(object o, bool encoded = false)
        {            
            if(encoded)
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o)));
            return JsonConvert.SerializeObject(o);
        }
    }
}
