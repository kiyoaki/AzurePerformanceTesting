using System.Threading.Tasks;
using System.Web.Mvc;
using Core;
using Core.TestingDbAdapters;
using Core.TestingLogAdapters;

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

            var logger = TestingLogAdapterFactory.Create(logAdapterType);
            var task = Task.Factory.StartNew(() =>
            {
                TestFunctions.WriteAndReadDatabase(adapterType, connectionString, writeCount, readCount, maxThreadCount, logger);
                ViewBag.StatusMessage = "Start Testing...";
            });
            task.ConfigureAwait(false);
            task.ContinueWith(x =>
            {
                var message = x.Exception != null ? x.Exception.Message : "";
                logger.Error("Exception: " + message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            return View("Index");
        }
    }
}