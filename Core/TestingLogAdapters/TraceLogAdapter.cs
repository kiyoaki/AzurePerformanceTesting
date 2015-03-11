using System.Diagnostics;

namespace AzureSqlDatabaseStressTestTool
{
    public class TraceLogAdapter : ITestingLogAdapter
    {
        public void Info(string logMessage)
        {
            Trace.TraceInformation(logMessage);
        }

        public void Info(string logMessage, params object[] args)
        {
            Trace.TraceInformation(logMessage, args);
        }

        public void Error(string logMessage)
        {
            Trace.TraceError(logMessage);
        }

        public void Error(string logMessage, params object[] args)
        {
            Trace.TraceError(logMessage, args);
        }
    }
}
