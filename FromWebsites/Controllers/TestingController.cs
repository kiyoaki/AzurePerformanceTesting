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

            TestFunctions.WriteAndReadDatabaseAsync(adapterType,
                connectionString, writeCount, readCount, maxThreadCount, logger);

            ViewBag.StatusMessage = "Start Testing...";

            return View("Index");
        }
    }
}