using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using SpeedControlSystemWeb.Models;

namespace SpeedControlSystemWeb.Controllers
{
    [ApiController]
    [Route("request")]
    public class SpeedRequestController : ControllerBase
    {
 

        public SpeedRequestController()
        {
        }

        bool CheckTime()
        {
            string timeStart = System.Configuration.ConfigurationManager.AppSettings.Get("WorkTimeStart")!;
            string timeEnd = System.Configuration.ConfigurationManager.AppSettings.Get("WorkTimeEnd")!;
            DateTime dateTimeNow = DateTime.Now;

            DateTime dateTimeStart = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, int.Parse(timeStart.Split(':')[0]), int.Parse(timeStart.Split(':')[1]), 0);
            DateTime dateTimeEnd = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, int.Parse(timeEnd.Split(':')[0]), int.Parse(timeEnd.Split(':')[1]), 0);
            return dateTimeStart < dateTimeNow && dateTimeEnd > dateTimeNow;
        }

        [HttpGet("speed")]
        public object GetSpeed()
        {
            if (CheckTime())
            {
                return new Responce(DataSearcher.Search(speed: 400, DateTime.Now.Date.AddDays(-1), null));
            }
            else
            {
                return new Responce();
            }

        }

        [HttpGet("date")]
        public object GetDate()
        {

            if (CheckTime())
            {
                return DataSearcher.Search(null, DateTime.Now.Date.AddDays(-2), DateTime.Now.Date.AddDays(-1));
            }
            else
            {
                return "Service unavailable at this time";
            }
        }
    }
}