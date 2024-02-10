
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;

namespace MyApiModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyApiController : ControllerBase
    {
        private readonly IContentManager _contentManager;

        public MyApiController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var contentItems = await _contentManager.Query().ListAsync();
            return Ok(contentItems);
        }
    }
}
