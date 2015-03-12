namespace AzureSqlDatabaseStressTestTool
{
    public enum TestingDbAdapterType
    {
        Dapper = 0,
        EntityFramework = 10,
        RawAdoNet = 20,
        BulkCopy = 30,
        StackExchangeRedis = 100,
    }
}
