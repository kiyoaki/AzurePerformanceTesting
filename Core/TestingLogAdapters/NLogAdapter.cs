using NLog;

namespace AzureSqlDatabaseStressTestTool
{
    public class NLogAdapter : ITestingLogAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Info(string logMessage)
        {
            Logger.Info(logMessage);
        }

        public void Info(string logMessage, params object[] args)
        {
            Logger.Info(logMessage, args);
        }

        public void Error(string logMessage)
        {
            Logger.Error(logMessage);
        }

        public void Error(string logMessage, params object[] args)
        {
            Logger.Error(logMessage, args);
        }
    }
}
