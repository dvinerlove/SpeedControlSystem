

using Bsa.Search.Core;
using Bsa.Search.Core.Common.Dictionaries;
using Bsa.Search.Core.Common.Entities;
using Bsa.Search.Core.Common.Readers;
using Bsa.Search.Core.Documents;
using Bsa.Search.Core.Helpers;
using Bsa.Search.Core.Indexes;
using Bsa.Search.Core.Indexes.Requests;
using Bsa.Search.Core.Queries.Helpers;
using Bsa.Search.Core.Terms;
using Bsa.Search.Core.Terms.Selectors;
using ClassLibrary;
using SpeedControlSystem;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;



 Random random = new Random();
const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0";
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

Console.WriteLine("Start!");
CreateFile();

//Request test = new RequestDate(DateTime.Now, 200);
////ReadFile(test);
////test = new RequestSpeed(DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-5));
//ReadFile(test);


stopWatch.Stop();
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("RunTime " + elapsedTime);
Console.WriteLine("End!");
Console.ReadKey();
//8652680
//50000000
void CreateFile()
{

    using (StreamWriter stream = new FileInfo("D:\\Test1.csv").AppendText())
    {
        for (int i = 0; i < 5000; i++)
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-random.Next(0, 90));

            var report = new SpeedReport()
            {
                Id = i,
                DateTime = dateTime.Ticks,
                Number = $"{random.Next(1000, 9999)} {chars[random.Next(chars.Length)]}{chars[random.Next(chars.Length)]}-{random.Next(1, 8)}",
                Speed = Math.Round(random.Next(10, 250) + random.NextDouble(), 2),
            };
            stream.WriteLine($"{report.Id},{report.DateTime},{report.Number},{report.Speed.ToString().Replace(",", ".")}");

        }
    }

}


//static void ReadFile(Request request)
//{
//    XmlReaderSettings settings = new XmlReaderSettings();
//    settings.DtdProcessing = DtdProcessing.Parse;
//    XmlReader reader = XmlReader.Create("D:\\Test.xml", settings);

//    List<SpeedReport> reportList = new List<SpeedReport>();

//    reader.MoveToContent();
//    while (reader.Read())
//    {
//        if (reader.NodeType == XmlNodeType.Text)
//        {
//            var report = new SpeedReport(reader.Value);
//            if (report.ProcessRequest(request))
//                reportList.Add(report);
//        }
//    }
//    Console.WriteLine("Searched items: {0}", reportList.Count);

//    //foreach (var item in reportList)
//    //{
//    //    Console.WriteLine(item.ToString());
//    //}
//}
