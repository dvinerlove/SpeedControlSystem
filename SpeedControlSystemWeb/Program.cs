using Microsoft.Web.Administration;
using SpeedControlSystemWeb.Models;
using System.Configuration;
using System.Diagnostics;



string folderPath = System.Configuration.ConfigurationManager.AppSettings.Get("DataFolderPath")!;
Debug.WriteLine(folderPath);
DataSearcher.FolderPath = folderPath;
DataSearcher.IndexFile();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
