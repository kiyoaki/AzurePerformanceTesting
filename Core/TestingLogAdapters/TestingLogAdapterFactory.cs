using System;
using System.IO;

namespace Core.TestingLogAdapters
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

                case TestingLogAdapterType.Console:
                    return new ConsoleLogAdapter();

                default:
                    throw new InvalidOperationException(adapterType + " is not implemented.");
            }
        }
    }
}
