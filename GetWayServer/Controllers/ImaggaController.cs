using DP;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace GetWayServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaggaController : ControllerBase
    {
        // GET: api/<ImaggaController>
        [HttpGet]
        public string Get(string title, Byte[] imageURL)
        {
            BL.ImaggaLogic bl = new BL.ImaggaLogic();
            ImaggaParamsDTO dp = new ImaggaParamsDTO { Title = title, ImageByte = imageURL };
            return bl.IsGoodPic(dp);
        }
    }
    
}
