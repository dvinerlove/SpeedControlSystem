using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SpeedControlSystem
{
    public class SpeedReport1
    {
        [XmlElement("DateTime")]
        public DateTime DateTime { get; set; }
        [XmlElement("Number")]
        public string Number { get; set; }
        [XmlElement("Speed")]
        public double Speed { get; set; }

        public SpeedReport1(string value)
        {
            var report = JsonConvert.DeserializeObject<SpeedReport1>(value);

            DateTime = report!.DateTime;
            Number = report.Number;
            Speed = report.Speed;
        }

        public SpeedReport1()
        {
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        internal bool ProcessRequest(Request request)
        {
            switch (request.RquestType)
            {
                case RquestType.Date:
                    RequestDate requestDate = (RequestDate)request;
                    return requestDate.MinSpeed < Speed && requestDate.DateMain.Date.Equals(DateTime.Date);
                case RquestType.Speed:
                    RequestSpeed requestSpeed = (RequestSpeed)request;
                    return DateTime > requestSpeed.DateMain && DateTime < requestSpeed.DateSecond;
            }
            return false;
        }
    }
}
