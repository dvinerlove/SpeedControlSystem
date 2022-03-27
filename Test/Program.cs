using ClassLibrary;
using Newtonsoft.Json;
using RestSharp;


var client = new RestClient($"http://localhost:5027/request/add");
SpeedReport speedReport = new SpeedReport(DateTime.Now, "1234 MM-5", 160) { };
NewMethod(client, JsonConvert.SerializeObject(speedReport));

client = new RestClient($"http://localhost:5027/request/speed");
ApiRequest apiRequest = new ApiRequestSpeed(DateTime.Now.AddDays(0), 160);
NewMethod(client, JsonConvert.SerializeObject(apiRequest));

client = new RestClient($"http://localhost:5027/request/date");
apiRequest = new ApiRequestDate(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(+1));
NewMethod(client, JsonConvert.SerializeObject(apiRequest));


static void NewMethod(RestClient client, string json)
{ 
    Console.WriteLine();
    var request1 = new RestRequest(Method.POST);
    request1.AddJsonBody(json);
    var resp = client.Execute(request1);
    try
    {
        ApiResponce apiResponce = JsonConvert.DeserializeObject<ApiResponce>(resp.Content)!;

        Console.WriteLine(apiResponce.StateMessage);
        Console.WriteLine();

        if (apiResponce.ResponceState == ResponceState.Success)
        {
            Console.WriteLine("Count "+apiResponce.Count);
            Console.WriteLine();
            foreach (var item in apiResponce.Data)
            {
                Console.WriteLine(item);
            }
        }
    }
    catch 
    {
        Console.WriteLine(resp.Content);

    }

    Console.ReadLine();
}