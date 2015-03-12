using System;
using System.IO;

namespace AzureSqlDatabaseStressTestTool
{
    public class TextWriterLogAdapter : ITestingLogAdapter
    {
        private readonly TextWriter _writer;

        public TextWriterLogAdapter(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            _writer = writer;
        }

        public void Info(string logMessage)
        {
            _writer.WriteLine(logMessage);
        }

        public void Info(string logMessage, params object[] args)
        {
            _writer.WriteLine(logMessage, args);
        }

        public void Error(string logMessage)
        {
            _writer.WriteLine(logMessage);
        }

        public void Error(string logMessage, params object[] args)
        {
            _writer.WriteLine(logMessage, args);
        }
    }
}
