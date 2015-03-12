using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.TestingDbAdapters;
using Core.TestingLogAdapters;
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
            var logger = TestingLogAdapterFactory.Create(LogAdapterType);

            try
            {
                TestFunctions.WriteAndReadDatabase(AdapterType, ConnectionString,
                    WriteCount, ReadCount, MaxThreadCount, logger);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message);
            }
        }
    }
}
