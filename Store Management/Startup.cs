using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Store_Management.Startup))]
namespace Store_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
