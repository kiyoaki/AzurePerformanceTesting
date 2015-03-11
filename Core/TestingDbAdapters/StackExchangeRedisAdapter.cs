using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AzureSqlDatabaseStressTestTool
{
    public class StackExchangeRedisAdapter : ITestingDbAdapter
    {
        private readonly ConnectionMultiplexer _connection;

        public StackExchangeRedisAdapter(string connectionString)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }

        public void DropAndCreateTable()
        {
        }

        public void Insert(Testing entity)
        {
            var db = _connection.GetDatabase();
            var json = JsonConvert.SerializeObject(entity);
            db.StringSet(entity.Name, json);
        }

        public Testing Select()
        {
            var db = _connection.GetDatabase();
            var id = new Random().Next(100);
            var key = TestingConstants.NamePrefix + id;
            var json = (string)db.StringGet(key);
            return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<Testing>(json);
        }
    }
}
