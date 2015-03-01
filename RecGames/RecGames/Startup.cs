using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecGames.Startup))]
namespace RecGames
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
