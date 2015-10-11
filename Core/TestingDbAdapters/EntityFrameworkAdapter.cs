using System;
using System.Data.Entity;
using System.Linq;

namespace Core.TestingDbAdapters
{
    public class EntityFrameworkAdapter : TestingDbAdapter
    {
        public EntityFrameworkAdapter(string connectionString)
            : base(connectionString)
        {
        }

        public override void Insert(Testing entity, string tableName)
        {
            using (var db = new Entities(ConnectionString))
            {
                db.Testing.Add(entity);
                db.SaveChanges();
            }
        }

        public override Testing Select(string tableName)
        {
            using (var db = new Entities(ConnectionString))
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