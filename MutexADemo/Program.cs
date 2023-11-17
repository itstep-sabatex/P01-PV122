// See https://aka.ms/new-console-template for more information
Console.WriteLine("Mutex A!");
Mutex mutex = new Mutex(false, "DemoMutex");

for  (int i = 0; i < 10; i++)
{
    if (mutex.WaitOne())
    {
        Console.WriteLine($"Mutex A {i} ");
        Thread.Sleep(1000);
        mutex.ReleaseMutex();
        
    }
    
}