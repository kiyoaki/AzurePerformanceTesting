using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Core.TestingDbAdapters
{
    public class DapperAdapter : TestingDbAdapter
    {
        public DapperAdapter(string connectionString)
            : base(connectionString)
        {
        }

        public override void Insert(Testing entity, string tableName)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(InsertSql(tableName), entity);
            }
        }

        public override Testing Select(string tableName)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<Testing>(SelectSql(tableName), new { Id = new Random().Next(100) })
                    .FirstOrDefault();
            }
        }
    }
}
