using Microsoft.AspNetCore.Mvc;
using RestSharp;


namespace GetWayServer.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class HebCalController : ControllerBase
        {
            // GET: api/<HebCalController>
            [HttpGet]
            public List<string> Get()
            {
                string today = "2023-09-27";  // DateTime.Today.ToString("yyyy-MM-dd");
                string SevenDaysFromNow = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
                BL.HebCalLogic bl = new BL.HebCalLogic();
                return bl.IsHolidyWeek(today, SevenDaysFromNow);
            }


        }
    
}
