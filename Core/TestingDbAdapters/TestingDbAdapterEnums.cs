namespace Core.TestingDbAdapters
{
    public enum TestingDbAdapterType
    {
        Dapper = 0,
        EntityFramework = 10,
        RawAdoNet = 20,
        BulkCopy = 30,
        StackExchangeRedis = 100,
        NoSerializingStackExchangeRedis = 110
    }
}
