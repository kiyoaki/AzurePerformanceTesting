using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace Core.TestingDbAdapters
{
    public class BulkCopyAdapter : TestingDbAdapter
    {
        private readonly string _connectionString;
        private readonly int _writeCount;
        private static ConcurrentBag<Testing> _bag = new ConcurrentBag<Testing>();

        public BulkCopyAdapter(string connectionString, int writeCount)
            : base(connectionString)
        {
            _connectionString = connectionString;
            _writeCount = writeCount;
        }

        public override void Insert(Testing entity)
        {
            _bag.Add(entity);

            if (_bag.Count < _writeCount) return;

            using (var conn = new SqlConnection(_connectionString))
            using (var bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.DestinationTableName = TestingConstants.TableName;
                conn.Open();

                var dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Name", typeof(string));
                dataTable.Columns.Add("TreadId", typeof(int));
                dataTable.Columns.Add("AddTime", typeof(DateTime));
                foreach (var testing in _bag)
                {
                    var row = dataTable.NewRow();
                    row["Id"] = testing.Id;
                    row["Name"] = testing.Name;
                    row["TreadId"] = testing.TreadId;
                    row["AddTime"] = testing.AddTime;
                    dataTable.Rows.Add(row);
                }

                _bag = new ConcurrentBag<Testing>();
                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
