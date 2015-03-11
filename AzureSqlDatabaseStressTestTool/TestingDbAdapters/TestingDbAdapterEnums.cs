namespace AzureSqlDatabaseStressTestTool
{
    public enum TestingDbAdapterType
    {
        Dapper = 0,
        EntityFramework = 10,
        RawAdoNet = 20,
        StackExchangeRedis = 100,
        
    }
}
