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

        public override void Insert(Testing entity, string tableName)
        {
            var db = _connection.GetDatabase();
            var key = tableName + "-" + entity.Id;
            db.StringSet(key, 1);
        }

        public override Testing Select(string tableName)
        {
            var db = _connection.GetDatabase();
            var id = new Random().Next(100);
            var key = tableName + "-" + id;
            var value = db.StringGet(key);
            return new Testing();
        }
    }
}
