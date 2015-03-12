using StackExchange.Redis;

namespace Core.Serializers
{
    public interface ISerializer
    {
        RedisValue Serialize(object value);
        T Deserialize<T>(string value);
    }
}
