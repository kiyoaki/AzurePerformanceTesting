using System.IO;
using ProtoBuf;
using StackExchange.Redis;

namespace Core.Serializers
{
    public class ProtocolBuffersSerializer : ISerializer
    {
        public RedisValue Serialize(object value)
        {
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                Serializer.Serialize(stream, value);
                stream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
        }

        public T Deserialize<T>(string value)
        {
            using (var stream = new MemoryStream())
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}
