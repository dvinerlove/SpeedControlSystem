using SpeedControlSystemWeb.Models;
using System.Diagnostics;

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

Debug.WriteLine("Start!");

string folderPath = System.Configuration.ConfigurationManager.AppSettings.Get("DataFolderPath")!;
DataSearcher.FolderPath = folderPath;
string filename = System.Configuration.ConfigurationManager.AppSettings.Get("DataFilename")!;
DataSearcher.Filename= filename;
DataSearcher.IndexFile();

stopWatch.Stop();
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Debug.WriteLine("RunTime " + elapsedTime);
Debug.WriteLine("End!");


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
