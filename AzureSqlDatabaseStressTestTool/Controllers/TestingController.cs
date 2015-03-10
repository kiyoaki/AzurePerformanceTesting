using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;

namespace AzureSqlDatabaseStressTestTool.Controllers
{
    public class TestingController : Controller
    {
        // GET: Testing
        public ActionResult Index(
            string connectionString,
            string table,
            int writeCount = 10000,
            int readCount = 10000,
            int maxThreadCount = 20)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                ViewBag.StatusMessage = "Input ConnectionString";
                return View();
            }

            if (string.IsNullOrWhiteSpace(table))
            {
                table = "Testing";
            }

            var dropSql = string.Format(
@"if exists (select * from sysobjects where id =
object_id(N'[dbo].[{0}]') and
  OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[{0}];", table);

            var createSql = string.Format(
@"CREATE TABLE [dbo].[{0}] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX) NULL,
    [TreadId] INT            NOT NULL,
    [AddTime] DATETIME       NOT NULL
);", table);

            var task = Task.Factory.StartNew(() =>
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Execute(dropSql);
                    conn.Execute(createSql);
                }

                Trace.TraceInformation("Start connectionString: " + connectionString);

                var stopwatch = Stopwatch.StartNew();
                Parallel.ForEach(Enumerable.Range(1, writeCount), new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxThreadCount
                }, i =>
                {
                    var threadId = Environment.CurrentManagedThreadId;

                    using (var conn = new SqlConnection(connectionString))
                    {
                        var sql = string.Format("INSERT INTO {0} VALUES (@Name, @TreadId, @AddTime)", table);
                        conn.Execute(sql, new Entity
                        {
                            Name = "test" + i,
                            TreadId = threadId,
                            AddTime = DateTime.Now
                        });
                    }
                });

                stopwatch.Stop();

                Trace.TraceInformation("{0}rows written in {1}ms", writeCount, stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();

                Parallel.ForEach(Enumerable.Range(1, readCount), new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxThreadCount
                }, i =>
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        var a = conn.Query<Entity>("select Id = @Id", new { Id = new Random().Next(100) });
                    }
                });

                stopwatch.Stop();

                Trace.TraceInformation("{0}rows read in {1}ms", readCount, stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
            });

            task.ConfigureAwait(false);
            task.ContinueWith(x =>
            {
                var message = x.Exception != null ? x.Exception.Message : "";
                Trace.TraceError("Exception: " + message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            ViewBag.StatusMessage = "Start Testing... Trace at Azure Websites streaming log.";
            return View();
        }
    }

    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TreadId { get; set; }
        public DateTime AddTime { get; set; }
    }
}