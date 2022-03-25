using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    internal class Program
    {

        static Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0";

        static void Main(string[] args)
        {
            using (StreamWriter stream = new FileInfo("D:\\Test1.csv").AppendText())//ur file location//.AppendText())
            {
                for (int i = 0; i < 5000; i++)
                {
                    DateTime dateTime = DateTime.Now.Date;
                    dateTime = dateTime.AddDays(-random.Next(0, 90));

                    var report = new SpeedReport()
                    {
                        Id = i,
                        DateTime = dateTime,
                        Number = $"{random.Next(1000, 9999)} {chars[random.Next(chars.Length)]}{chars[random.Next(chars.Length)]}-{random.Next(1, 8)}",
                        Speed = Math.Round(random.Next(10, 250) + random.NextDouble(), 2),
                    }; 
                    stream.WriteLine($"{report.Id},{report.DateTime},{report.Number},{report.Speed.ToString().Replace(",", ".")}");

                }
            }
        }

    }
}
