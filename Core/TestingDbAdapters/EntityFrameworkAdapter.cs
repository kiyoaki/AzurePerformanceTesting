using System;
using System.Data.Entity;
using System.Linq;

namespace AzureSqlDatabaseStressTestTool
{
    public class EntityFrameworkAdapter : ITestingDbAdapter
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

        public EntityFrameworkAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DropAndCreateTable()
        {
            using (var db = new Entities(_connectionString))
            {
                db.Database.ExecuteSqlCommand(DropSql);
                db.Database.ExecuteSqlCommand(CreateTableSql);
            }
        }

        public void Insert(Testing entity)
        {
            using (var db = new Entities(_connectionString))
            {
                db.Testing.Add(entity);
                db.SaveChanges();
            }
        }

        public Testing Select()
        {
            using (var db = new Entities(_connectionString))
            {
                var id = new Random().Next(100);
                return db.Testing.FirstOrDefault(x => x.Id == id);
            }
        }
    }

    public class Entities : DbContext
    {
        public Entities(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Testing> Testing { get; set; }
    }
}