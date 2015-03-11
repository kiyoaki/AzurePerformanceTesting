using System;

namespace AzureSqlDatabaseStressTestTool
{
    public static class TestingLogAdapterFactory
    {
        public static ITestingLogAdapter Create(TestingLogAdapterType adapterType)
        {
            switch (adapterType)
            {
                case TestingLogAdapterType.Trace:
                    return new TraceLogAdapter();

                case TestingLogAdapterType.NLog:
                    return new NLogAdapter();

                default:
                    throw new InvalidOperationException(adapterType + " is not implemented.");
            }
        }
    }
}
