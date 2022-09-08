using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoDealership.Startup))]
namespace AutoDealership
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
