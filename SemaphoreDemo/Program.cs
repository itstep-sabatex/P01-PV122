// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var semaphore  = new Semaphore(3, 3);
for (int i = 0; i < 20; i++)
{
    int a = i;
    Task.Run(() =>
    {
        semaphore.WaitOne();
        Console.WriteLine($"Task {a} started");
        Thread.Sleep( 1000 );
        semaphore.Release();
    });    
}

Console.WriteLine("All task started");
Console.ReadLine();