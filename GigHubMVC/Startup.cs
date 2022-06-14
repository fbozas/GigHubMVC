using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GigHubMVC.Startup))]
namespace GigHubMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
