using System;
using System.IO;

namespace AzureSqlDatabaseStressTestTool
{
    public static class TestingLogAdapterFactory
    {
        public static ITestingLogAdapter Create(TestingLogAdapterType adapterType, TextWriter writer = null)
        {
            switch (adapterType)
            {
                case TestingLogAdapterType.Trace:
                    return new TraceLogAdapter();

                case TestingLogAdapterType.NLog:
                    return new NLogAdapter();

                case TestingLogAdapterType.TextWriter:
                    return new TextWriterLogAdapter(writer);

                case TestingLogAdapterType.Console:
                    return new ConsoleLogAdapter();

                default:
                    throw new InvalidOperationException(adapterType + " is not implemented.");
            }
        }
    }
}
