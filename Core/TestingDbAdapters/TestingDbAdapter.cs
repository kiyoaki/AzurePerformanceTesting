using System;
using System.Data.SqlClient;

namespace Core.TestingDbAdapters
{
    public abstract class TestingDbAdapter
    {
        protected readonly string ConnectionString;

        protected static readonly string CreateTableSql =
            string.Format(
@"CREATE TABLE [dbo].[{0}] (
[Id]      INT            IDENTITY (1, 1) NOT NULL,
[Name]    NVARCHAR (MAX) NULL,
[TreadId] INT            NOT NULL,
[AddTime] DATETIME       NOT NULL, 
CONSTRAINT [PK_Testing] PRIMARY KEY ([Id])
);", TestingConstants.TableName);

        protected static string CreateDropSql(string tableName)
        {
            return string.Format(
@"if exists (select * from sysobjects where id =
object_id(N'[dbo].[{0}]') and
    OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[{0}];", tableName);
        }

        protected static readonly string InsertSql =
            string.Format("INSERT INTO {0} VALUES (@Name, @TreadId, @AddTime)", TestingConstants.TableName);

        protected static readonly string SelectSql =
            string.Format("SELECT * FROM {0} WHERE Id = @Id", TestingConstants.TableName);

        protected TestingDbAdapter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual void DropAndCreateTable(string tableName)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = CreateDropSql(tableName);
                    command.ExecuteNonQuery();
                }

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = CreateTableSql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public abstract void Insert(Testing entity);

        public virtual Testing Select()
        {
            using (var conn = new SqlConnection(ConnectionString))
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
