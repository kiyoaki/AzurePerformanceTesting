﻿using System;

namespace Core.TestingDbAdapters
{
    public static class TestingDbAdapterFactory
    {
        public static TestingDbAdapter Create(TestingDbAdapterType adapterType, string connectionString, int writeCount)
        {
            switch (adapterType)
            {
                case TestingDbAdapterType.Dapper:
                    return new DapperAdapter(connectionString);

                case TestingDbAdapterType.EntityFramework:
                    return new EntityFrameworkAdapter(connectionString);

                case TestingDbAdapterType.RawAdoNet:
                    return new RawAdoNetAdapter(connectionString);

                case TestingDbAdapterType.BulkCopy:
                    return new BulkCopyAdapter(connectionString, writeCount);

                case TestingDbAdapterType.StackExchangeRedis:
                    return new StackExchangeRedisAdapter(connectionString, writeCount);

                case TestingDbAdapterType.NoSerializingStackExchangeRedis:
                    return new NoSerializingStackExchangeRedisAdapter(connectionString);

                default:
                    throw new InvalidOperationException(adapterType + " is not implemented.");
            }
        }
    }
}
