using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureSqlDatabaseStressTestTool.Startup))]
namespace AzureSqlDatabaseStressTestTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
