namespace ClassLibrary
{
    public enum RquestType
    {
        None,
        Date,
        Speed
    }
    public abstract class ApiRequest
    {
        public ApiRequest(RquestType rquestType)
        {
            RequestType = rquestType;
        }
        public DateTime DateMain { get; set; }
        public RquestType RequestType { get; set; } = RquestType.None;
    }
    public class ApiRequestSpeed : ApiRequest
    {
        public double MinSpeed { get; set; }
        public ApiRequestSpeed() : base(RquestType.Speed)
        {

        }
        public ApiRequestSpeed(DateTime dateTime, double minSpeed) : base(RquestType.Speed)
        {
            MinSpeed = minSpeed;
            DateMain = dateTime;
        }

    }
    public class ApiRequestDate : ApiRequest
    {
        public DateTime DateSecond { get; set; }
        public ApiRequestDate() : base(RquestType.Date)
        {

        }
        public ApiRequestDate(DateTime dateFirst, DateTime dateSecond) : base(RquestType.Date)
        {
            if (dateFirst > dateSecond)
            {
                var dateTmp = dateFirst;
                dateFirst = dateSecond;
                DateSecond = dateTmp;
                //throw new Exception("dateFirst must be greater then dateSecond");
            }
            DateSecond = dateSecond;
            DateMain = dateFirst;
        }
    }

}
