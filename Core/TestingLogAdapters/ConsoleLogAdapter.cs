using System;

namespace AzureSqlDatabaseStressTestTool
{
    public class ConsoleLogAdapter : ITestingLogAdapter
    {
        public void Info(string logMessage)
        {
            Console.WriteLine(logMessage);
        }

        public void Info(string logMessage, params object[] args)
        {
            Console.WriteLine(logMessage, args);
        }

        public void Error(string logMessage)
        {
            Console.WriteLine(logMessage);
        }

        public void Error(string logMessage, params object[] args)
        {
            Console.WriteLine(logMessage, args);
        }
    }
}
