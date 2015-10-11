using System;
using Core.Serializers;
using StackExchange.Redis;

namespace Core.TestingDbAdapters
{
    public class StackExchangeRedisAdapter : TestingDbAdapter
    {
        private readonly int _writeCount;
        private static readonly ISerializer Serializer = SerializerFactory.Create(SerializerType.ProtocolBuffers);
        private static volatile int _counter;
        private readonly ConnectionMultiplexer _connection;

        public StackExchangeRedisAdapter(string connectionString, int writeCount)
            : base(connectionString)
        {
            _writeCount = writeCount;
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }

        public override void DropAndCreateTable(string tableName)
        {
        }

        public override void Insert(Testing entity, string tableName)
        {
            entity.Id = _counter++;

            var db = _connection.GetDatabase();
            var json = Serializer.Serialize(entity);
            var key = tableName + "-" + entity.Id;
            db.StringSet(key, json);

            if (_counter >= _writeCount)
            {
                _counter = 0;
            }
        }

        public override Testing Select(string tableName)
        {
            var db = _connection.GetDatabase();
            var id = new Random().Next(100);
            var key = "tableName" + "-" + id;
            var json = (string)db.StringGet(key);
            return string.IsNullOrEmpty(json) ? null : Serializer.Deserialize<Testing>(json);
        }
    }
}
