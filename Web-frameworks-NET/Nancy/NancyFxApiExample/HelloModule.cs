
using Nancy;

namespace NancyFxApiExample
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get("/", _ => "Hello World from NancyFx!");
            Get("/hello/{name}", parameters => $"Hello {parameters.name} from NancyFx!");
        }
    }
}
