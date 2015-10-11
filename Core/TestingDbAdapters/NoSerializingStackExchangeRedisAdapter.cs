using System;
using StackExchange.Redis;

namespace Core.TestingDbAdapters
{
    public class NoSerializingStackExchangeRedisAdapter : TestingDbAdapter
    {
        private readonly ConnectionMultiplexer _connection;

        public NoSerializingStackExchangeRedisAdapter(string connectionString)
            : base(connectionString)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }

        public override void DropAndCreateTable(string tableName)
        {
        }

        public override void Insert(Testing entity)
        {
            var db = _connection.GetDatabase();
            var key = TestingConstants.RedisKeyPrefix + entity.Id;
            db.StringSet(key, 1);
        }

        public override Testing Select()
        {
            var db = _connection.GetDatabase();
            var id = new Random().Next(100);
            var key = TestingConstants.RedisKeyPrefix + id;
            var value = db.StringGet(key);
            return new Testing();
        }
    }
}
