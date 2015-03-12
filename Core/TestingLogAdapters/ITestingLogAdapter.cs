namespace Core.TestingLogAdapters
{
    public interface ITestingLogAdapter
    {
        void Info(string logMessage);

        void Info(string logMessage, params object[] args);

        void Error(string logMessage);

        void Error(string logMessage, params object[] args);
    }
}
