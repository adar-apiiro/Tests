using Microsoft.AspNetCore.Mvc;
using BlazorWebServerAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private static readonly List<TodoItem> items = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Do laundry", IsComplete = false },
            new TodoItem { Id = 2, Title = "Clean house", IsComplete = false }
        };

        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Post(TodoItem item)
        {
            items.Add(item);
            retur

public void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddSingleton<WeatherForecastService>();
    services.AddControllers(); // Add this to support API controllers
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Other configurations...

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers(); // Map API controllers
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
    });
}
