﻿using System.Data.SqlClient;

namespace Core.TestingDbAdapters
{
    public class RawAdoNetAdapter : TestingDbAdapter
    {
        public RawAdoNetAdapter(string connectionString)
            : base(connectionString)
        {
        }

        public override void Insert(Testing entity, string tableName)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = InsertSql(tableName);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@TreadId", entity.TreadId);
                    command.Parameters.AddWithValue("@AddTime", entity.AddTime);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
