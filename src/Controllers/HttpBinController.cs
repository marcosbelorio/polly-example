using HttpClientPollyExample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HttpClientPollyExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpBinController : ControllerBase
    {
        private readonly IHttpBinService _service;

        public HttpBinController(IHttpBinService service)
        {
            _service = service;
        }

        [HttpGet("{code:int}")]
        public async Task<IActionResult> Get([FromRoute] int code)
        {
            await _service.Get(code);
            return Ok();
        }
    }
}
