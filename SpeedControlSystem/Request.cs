using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedControlSystem
{
    public enum RquestType
    {
        None,
        Date,
        Speed
    }
    public abstract class Request
    {
        public Request(RquestType rquestType)
        {
            RquestType = rquestType;
        }
        public DateTime DateMain { get; set; }
        public RquestType RquestType { get; set; } = RquestType.None;
    }
    public class RequestDate : Request
    {
        public double MinSpeed { get; set; }

        public RequestDate(DateTime dateTime, double minSpeed) : base(RquestType.Date)
        {
            MinSpeed = minSpeed;
            DateMain = dateTime;
        }
    }
    public class RequestSpeed : Request
    {
        public DateTime DateSecond { get; set; }

        public RequestSpeed(DateTime dateFirst, DateTime dateSecond) : base(RquestType.Speed)
        {
            if (dateFirst>dateSecond)
            {
                throw new Exception("dateFirst must be greater then dateSecond");
            }
            DateSecond = dateSecond;
            DateMain = dateFirst;
        }
    }

}
