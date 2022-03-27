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

        [HttpGet]
        public object GetSpeed()
        {
            return "hello";
        }


        [HttpPost("add")]
        public object AddNew([FromBody] SpeedReport speedReport)
        {
            bool exists = DataSearcher.AddItem(speedReport);
            if (exists)
            {
                return new ApiResponce(new List<string>() { speedReport.ToString()  });
            }
            else
            {
                return new ApiResponce(ResponceState.AlreadyExists);
            }
        }

        [HttpPost("speed")]
        public object PostSpeed([FromBody] ApiRequestSpeed apiRequestSpeed)
        {
            return GetResponce(apiRequestSpeed);
        }

        [HttpPost("date")]
        public object PostSpeed([FromBody] ApiRequestDate apiRequestDate)
        {
            return GetResponce(apiRequestDate);
        }

        private object GetResponce(ApiRequest apiRequest)
        {
            if (CheckTime())
            {
                List<string> result = new List<string>();
                switch (apiRequest.RequestType)
                {
                    case RquestType.Date:
                        ApiRequestDate apiRequestDate = (ApiRequestDate)apiRequest;
                        result = DataSearcher.Search(date: apiRequestDate.DateMain, dateMax: apiRequestDate.DateSecond);
                        break;
                    case RquestType.Speed:
                        ApiRequestSpeed apiRequestSpeed = (ApiRequestSpeed)apiRequest;
                        result = DataSearcher.Search(speed: apiRequestSpeed.MinSpeed, date: apiRequestSpeed.DateMain);
                        break;
                }
                return new ApiResponce(result);
            }
            else
            {
                return new ApiResponce( ResponceState.ServiceUnavailable);
            }
        }
    }
}