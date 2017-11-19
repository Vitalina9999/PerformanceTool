using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PerformanceTool.Startup))]
namespace PerformanceTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
