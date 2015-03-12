using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AzureSqlDatabaseStressTestTool.Controllers
{
    [RequireHttps]
    public class TestingController : Controller
    {
        // GET: Testing
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.StatusMessage = "Please input ConnectionString";
            return View();
        }

        [HttpPost]
        public ActionResult Start(
            TestingDbAdapterType adapterType,
            string connectionString,
            int writeCount = 10000,
            int readCount = 10000,
            int maxThreadCount = 20,
            TestingLogAdapterType logAdapterType = TestingLogAdapterType.Trace)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                ViewBag.StatusMessage = "Please input ConnectionString";
                return View("Index");
            }

            var adapter = TestingDbAdapterFactory.Create(adapterType, connectionString, writeCount);
            var logger = TestingLogAdapterFactory.Create(logAdapterType);

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
                        Name = TestingConstants.NamePrefix + i,
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

            ViewBag.StatusMessage = "Start Testing...";
            return View("Index");
        }
    }
}