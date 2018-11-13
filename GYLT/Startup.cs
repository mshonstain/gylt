using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GYLT.Startup))]
namespace GYLT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
