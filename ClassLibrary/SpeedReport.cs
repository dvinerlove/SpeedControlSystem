using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class SpeedReport
    {
        public int Id { get; set; }
        public long DateTime { get; set; }
        public string Number { get; set; }
        public double Speed { get; set; }
        public override string ToString()
        {
            return $"{new DateTime(DateTime).ToShortDateString()} {new DateTime(DateTime).ToShortTimeString()} {Number} {Speed}";
        }
    }
}
