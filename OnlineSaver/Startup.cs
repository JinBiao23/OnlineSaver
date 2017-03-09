using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineSaver.Startup))]
namespace OnlineSaver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
