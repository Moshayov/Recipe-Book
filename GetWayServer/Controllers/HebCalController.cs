using Microsoft.AspNetCore.Mvc;
using RestSharp;
using DP;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipesSystem.GetwayServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HebCalController : ControllerBase
    {
        // GET: api/<HebCalController>
        [HttpGet]
        public List<string> Get()
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string SevenDaysFromNow = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            BL.HebCalLogic bl = new BL.HebCalLogic();
            return bl.IsHolidyWeek(today, SevenDaysFromNow);
        }

   
    }
}
