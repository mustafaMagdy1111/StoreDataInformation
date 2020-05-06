using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StoreDataInformation.Startup))]
namespace StoreDataInformation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
