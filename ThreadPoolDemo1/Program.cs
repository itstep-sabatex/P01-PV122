// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata.Ecma335;

Console.WriteLine("Hello, World!");

for (int i = 0; i < 10; i++)
{
    var a = i;
    var r = await Task.Run(async () => { await Task.Delay(1000); Console.WriteLine($"Task {a}"); return 10; });
}

List<Task<string>> ts = new List<Task<string>>();
for (int i = 0;i < 10; i++)
{
    var a = i;
    ts.Add(Task.Run(async () => {  Console.WriteLine($"Task no await {a}");await Task.Delay(1000); return $"Task no await {a}"; }));
}
Console.WriteLine("All task started");
while (ts.Any(t => !t.IsCompleted))
{
    await Task.Delay(100);
}

Console.WriteLine("All task complited");
foreach (var t in ts)
{
    Console.WriteLine(t.Result);
}


Console.ReadLine();

Console.WriteLine(await Task.Run(() => { return 10; }));