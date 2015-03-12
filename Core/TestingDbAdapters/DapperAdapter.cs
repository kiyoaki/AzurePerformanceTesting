using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace AzureSqlDatabaseStressTestTool
{
    public class DapperAdapter : TestingDbAdapter
    {
        public DapperAdapter(string connectionString)
            : base(connectionString)
        {
        }

        public override void Insert(Testing entity)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(InsertSql, entity);
            }
        }

        public override Testing Select()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<Testing>(SelectSql, new { Id = new Random().Next(100) })
                    .FirstOrDefault();
            }
        }
    }
}
