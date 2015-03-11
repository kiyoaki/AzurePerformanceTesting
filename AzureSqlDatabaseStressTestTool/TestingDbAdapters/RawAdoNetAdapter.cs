using System;
using System.Data.SqlClient;

namespace AzureSqlDatabaseStressTestTool
{
    public class RawAdoNetAdapter : ITestingDbAdapter
    {
        private readonly string _connectionString;

        private static readonly string CreateTableSql =
            string.Format(
            @"CREATE TABLE [dbo].[{0}] (
                [Id]      INT            IDENTITY (1, 1) NOT NULL,
                [Name]    NVARCHAR (MAX) NULL,
                [TreadId] INT            NOT NULL,
                [AddTime] DATETIME       NOT NULL, 
                CONSTRAINT [PK_Testing] PRIMARY KEY ([Id])
            );", TestingConstants.TableName);

        private static readonly string DropSql =
            string.Format(
            @"if exists (select * from sysobjects where id =
            object_id(N'[dbo].[{0}]') and
              OBJECTPROPERTY(id, N'IsUserTable') = 1)
            DROP TABLE [dbo].[{0}];", TestingConstants.TableName);

        private static readonly string InsertSql =
            string.Format("INSERT INTO {0} VALUES (@Name, @TreadId, @AddTime)", TestingConstants.TableName);

        private static readonly string SelectSql =
            string.Format("SELECT * FROM {0} WHERE Id = @Id", TestingConstants.TableName);

        public RawAdoNetAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DropAndCreateTable()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = DropSql;
                    command.ExecuteNonQuery();
                }

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = CreateTableSql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Insert(Testing entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = InsertSql;
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@TreadId", entity.TreadId);
                    command.Parameters.AddWithValue("@AddTime", entity.AddTime);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Testing Select()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    var id = new Random().Next(100);
                    command.CommandText = SelectSql;
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read()
                            ? new Testing
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                TreadId = (int)reader["TreadId"],
                                AddTime = (DateTime)reader["AddTime"],
                            }
                            : null;
                    }
                }
            }
        }
    }
}
