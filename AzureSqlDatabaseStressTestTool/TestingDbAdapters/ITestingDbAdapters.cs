namespace AzureSqlDatabaseStressTestTool
{
    public interface ITestingDbAdapter
    {
        void DropAndCreateTable();

        void Insert(Testing entity);

        Testing Select();
    }
}
