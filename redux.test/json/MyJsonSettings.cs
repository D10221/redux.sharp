using Newtonsoft.Json;

namespace redux.test
{
    public class MyJsonSettings
    {
        public static void Configure()
        {           
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new ValueTupleContractResolver(),
                Converters = { new ValueOfJsonConverter() }
            };
        }
    }
}