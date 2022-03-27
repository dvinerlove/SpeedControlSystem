using ClassLibrary;
using System.Diagnostics;


Random random = new Random();
const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0";
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

Console.WriteLine("Start!");
CreateFile();

stopWatch.Stop();
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("RunTime " + elapsedTime);
Console.WriteLine("End!");
Console.ReadKey();
void CreateFile()
{

    using (StreamWriter stream = new FileInfo("D:\\SpeedControlSystemData\\Data.csv").AppendText())
    {
        for (int i = 0; i < 102109; i++)
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-random.Next(0, 90));

            var report = new SpeedReport()
            {
                DateTime = dateTime.Ticks,
                Number = $"{random.Next(1000, 9999)} {chars[random.Next(chars.Length)]}{chars[random.Next(chars.Length)]}-{random.Next(1, 8)}",
                Speed = Math.Round(random.Next(10, 250) + random.NextDouble(), 2),
            };
            if (i == 5)
            {
                Console.WriteLine(report.Hash);
                Console.WriteLine(report);
                Console.WriteLine(dateTime.Ticks);
            }
            stream.WriteLine($"{report.Hash},{report.DateTime},{report.Number},{report.Speed.ToString().Replace(",", ".")}");
        }
    }
}