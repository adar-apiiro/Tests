
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using System.Threading.Tasks;

namespace ServiceStackExample2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceStack(new AppHost());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseServiceStack(new AppHost());
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("New ServiceStack Web Services", typeof(MyServices).Assembly) { }

        public override void Configure(Container container)
        {
            // Add your ServiceStack configuration here
            SetConfig(new HostConfig { DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false) });
        }
    }

    [Route("/goodbye/{Name}", "GET")]
    public class GoodbyeRequest : IReturn<GoodbyeResponse>
    {
        public string Name { get; set; }
    }

    public class GoodbyeResponse
    {
        public string Result { get; set; }
    }

    public class MyServices : Service
    {
        public object Get(GoodbyeRequest request)
        {
            return new GoodbyeResponse { Result = $"Goodbye, {request.Name}!" };
        }
    }
}
