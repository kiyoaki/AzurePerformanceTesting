using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace AzureSqlDatabaseStressTestTool
{
    public class DapperAdapter : ITestingDbAdapter
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

        public DapperAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DropAndCreateTable()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute(DropSql);
                conn.Execute(CreateTableSql);
            }
        }

        public void Insert(Testing entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute(InsertSql, entity);
            }
        }

        public Testing Select()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Query<Testing>(SelectSql, new { Id = new Random().Next(100) })
                    .FirstOrDefault();
            }
        }
    }
}
