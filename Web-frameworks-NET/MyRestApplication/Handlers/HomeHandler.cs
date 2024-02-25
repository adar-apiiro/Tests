using MyRestApplication.Resources;

namespace MyRestApplication.Handlers
{
    public class HomeHandler
    {
        public object Get()
        {
            return new Home { Title = "Welcome home." };
        }
    }
}