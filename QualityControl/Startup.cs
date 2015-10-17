using Microsoft.Owin;
using Owin;
using QualityControl;

[assembly: OwinStartup(typeof (Startup))]

namespace QualityControl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}