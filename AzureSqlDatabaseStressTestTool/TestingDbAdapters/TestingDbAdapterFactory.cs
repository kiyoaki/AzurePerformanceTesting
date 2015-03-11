using System;

namespace AzureSqlDatabaseStressTestTool
{
    public static class TestingDbAdapterFactory
    {
        public static ITestingDbAdapter Create(TestingDbAdapterType adapterType, string connectionString)
        {
            switch (adapterType)
            {
                case TestingDbAdapterType.Dapper:
                    return new DapperAdapter(connectionString);

                case TestingDbAdapterType.EntityFramework:
                    return new EntityFrameworkAdapter(connectionString);

                case TestingDbAdapterType.StackExchangeRedis:
                    return new StackExchangeRedisAdapter(connectionString);

                default:
                    throw new InvalidOperationException(adapterType + " is not implemented.");
            }
        }
    }
}
