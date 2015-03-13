using System;
using System.Globalization;
using System.Linq;
using Core;
using Core.TestingDbAdapters;
using Core.TestingLogAdapters;

namespace FromConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null
                || args.Length <= 0
                || args[0].Equals("help", StringComparison.CurrentCultureIgnoreCase)
                || args[0].Equals("man", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine(CreateHelpString());
                Console.ReadKey();
                return;
            }

            var connectionString = args[0];
            var adapterType = TestingDbAdapterType.Dapper;
            var logAdapterType = TestingLogAdapterType.Console;
            var writeCount = 10000;
            var readCount = 10000;
            var maxThreadCount = 100;

            TestingDbAdapterType argsAdapterType;
            if (args.Length >= 2 && Enum.TryParse(args[1], out argsAdapterType))
            {
                adapterType = argsAdapterType;
            }

            TestingLogAdapterType argsLogAdapterType;
            if (args.Length >= 3 && Enum.TryParse(args[2], out argsLogAdapterType))
            {
                logAdapterType = argsLogAdapterType;
            }

            int argsWriteCount;
            if (args.Length >= 4 && int.TryParse(args[3], out argsWriteCount))
            {
                writeCount = argsWriteCount;
            }

            int argsReadCount;
            if (args.Length >= 5 && int.TryParse(args[4], out argsReadCount))
            {
                readCount = argsReadCount;
            }

            int argsMaxThreadCount;
            if (args.Length >= 6 && int.TryParse(args[5], out argsMaxThreadCount))
            {
                maxThreadCount = argsMaxThreadCount;
            }

            var logger = TestingLogAdapterFactory.Create(logAdapterType);

            try
            {
                TestFunctions.WriteAndReadDatabase(adapterType, connectionString,
                    writeCount, readCount, maxThreadCount, logger);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static string CreateHelpString()
        {
            var adapterTypeHelp = CreateEnumHelpString<TestingDbAdapterType>();
            var logAdapterTypeHelp = CreateEnumHelpString<TestingLogAdapterType>();

            return string.Join(Environment.NewLine + Environment.NewLine,
                new[]
                {
                    "AzurePerformanceTesting ConnectionString AdapterType LogAdapterType WriteCount ReadCount MaxThreadCount",
                    "    ConnectionString [Required]",
                    string.Format("    AdapterType [Default:Dapper] {0}{1}", Environment.NewLine, adapterTypeHelp),
                    string.Format("    LogAdapterType [Default:Console] {0}{1}", Environment.NewLine, logAdapterTypeHelp),
                    "    WriteCount [Default:10000]",
                    "    ReadCount [Default:10000]",
                    "    MaxThreadCount [Default:100]"
                });
        }

        static string CreateEnumHelpString<T>() where T :
            struct, IComparable, IConvertible, IFormattable
        {
            return string.Join(Environment.NewLine,
                Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Select(x => string.Format("        {0}:{1}",
                        x.ToString(), x.ToInt32(CultureInfo.CurrentCulture))));
        }
    }
}
