using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.Serializers
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        public RedisValue Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
