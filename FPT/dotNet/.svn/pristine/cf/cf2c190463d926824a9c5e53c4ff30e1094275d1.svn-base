using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CertMClient.Startup))]
namespace CertMClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
