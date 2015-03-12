using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AzureSqlDatabaseStressTestTool
{
    public class StackExchangeRedisAdapter : TestingDbAdapter
    {
        private readonly ConnectionMultiplexer _connection;

        public StackExchangeRedisAdapter(string connectionString)
            : base(connectionString)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }

        public override void DropAndCreateTable()
        {
        }

        public override void Insert(Testing entity)
        {
            var db = _connection.GetDatabase();
            var json = JsonConvert.SerializeObject(entity);
            db.StringSet(entity.Name, json);
        }

        public override Testing Select()
        {
            var db = _connection.GetDatabase();
            var id = new Random().Next(100);
            var key = TestingConstants.NamePrefix + id;
            var json = (string)db.StringGet(key);
            return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<Testing>(json);
        }
    }
}
