using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.TestingDbAdapters;
using Core.TestingLogAdapters;

namespace Core
{
    public static class TestFunctions
    {
        public static void WriteAndReadDatabase(
            TestingDbAdapterType adapterType,
            string connectionString,
            int writeCount,
            int readCount,
            int maxThreadCount,
            ITestingLogAdapter logger)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            var adapter = TestingDbAdapterFactory.Create(adapterType, connectionString, writeCount);

            var task = Task.Factory.StartNew(() =>
            {
                adapter.DropAndCreateTable();

                logger.Info("Start connectionString: " + connectionString);

                var stopwatch = Stopwatch.StartNew();
                Parallel.ForEach(Enumerable.Range(1, writeCount), new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxThreadCount
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

                logger.Info("{0}rows written in {1}ms", writeCount, stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();

                Parallel.ForEach(Enumerable.Range(1, readCount), new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxThreadCount
                }, i =>
                {
                    var a = adapter.Select();
                });

                stopwatch.Stop();

                logger.Info("{0}rows read in {1}ms", readCount, stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
            });

            task.ConfigureAwait(false);
            task.ContinueWith(x =>
            {
                var message = x.Exception != null ? x.Exception.Message : "";
                logger.Error("Exception: " + message);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
