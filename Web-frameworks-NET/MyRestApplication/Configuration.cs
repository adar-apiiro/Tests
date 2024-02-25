using OpenRasta.Configuration;
using MyRestApplication.Resources;
using MyRestApplication.Handlers;

namespace MyRestApplication
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<Home>()
                    .AtUri("/home")
                    .HandledBy<HomeHandler>()
                    .RenderedByAspx("~/Views/HomeView.aspx");
            }
        }
    }
}