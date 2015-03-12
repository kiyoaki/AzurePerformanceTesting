using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AzureSqlDatabaseStressTestTool;
using Microsoft.Azure.WebJobs;

namespace FromBatch
{
    public static class TestingTask
    {
        private const string ConnectionString = "Server=tcp:xxx.database.windows.net,1433;Database=xxx;User ID=xxx;Password=xxx;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        private const TestingDbAdapterType AdapterType = TestingDbAdapterType.Dapper;
        private const TestingLogAdapterType LogAdapterType = TestingLogAdapterType.Console;
        private const int WriteCount = 100000;
        private const int ReadCount = 100000;
        private const int MaxThreadCount = 100;

        [NoAutomaticTrigger]
        public static void Run()
        {
            var adapter = TestingDbAdapterFactory.Create(AdapterType, ConnectionString, WriteCount);
            var logger = TestingLogAdapterFactory.Create(LogAdapterType);

            try
            {
                adapter.DropAndCreateTable();

                logger.Info("Start connectionString: " + ConnectionString);

                var stopwatch = Stopwatch.StartNew();
                Parallel.ForEach(Enumerable.Range(1, WriteCount), new ParallelOptions
                {
                    MaxDegreeOfParallelism = MaxThreadCount
                }, i =>
                {
                    var threadId = Environment.CurrentManagedThreadId;
                    adapter.Insert(new Testing
                    {
                        Name = TestingConstants.RedisKeyPrefix + i,
                        TreadId = threadId,
                        AddTime = DateTime.Now
                    });
                });

                stopwatch.Stop();

                logger.Info("{0}rows written in {1}ms", WriteCount, stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();

                Parallel.ForEach(Enumerable.Range(1, ReadCount), new ParallelOptions
                {
                    MaxDegreeOfParallelism = MaxThreadCount
                }, i =>
                {
                    var a = adapter.Select();
                });

                stopwatch.Stop();

                logger.Info("{0}rows read in {1}ms", ReadCount, stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message);
            }
        }
    }
}
