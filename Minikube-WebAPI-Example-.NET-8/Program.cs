var builder = WebApplication.CreateBuilder(args);

string id = Guid.NewGuid().ToString();
string osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
DateTime startUpTime = DateTime.Now;
int requestCount = 0;

var app = builder.Build();

app.MapGet("/", () => $"Id {id}\nOperating System {osDescription}\nStarted at {startUpTime}\n\nCurrent time {DateTime.Now}\nRequest count {++requestCount}");

app.Run();