using System.Text;

namespace ClassLibrary
{
    public class SpeedReport
    {
        public SpeedReport(DateTime dateTime, string number, double speed)
        {
            DateTime = dateTime.Ticks;
            Number = number;
            Speed = speed;
        }
        public SpeedReport()
        {
        }

        public string Hash
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{DateTime}{Number}{Speed}"));
            }
        }
        public long DateTime { get; set; }
        public string Number { get; set; }
        public double Speed { get; set; }
        public override string ToString()
        {
            return $"{new DateTime(DateTime).ToShortDateString()} {new DateTime(DateTime).ToLongTimeString()} {Number} {Speed}";
        }
    }
}
