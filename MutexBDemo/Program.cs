// See https://aka.ms/new-console-template for more information
Console.WriteLine("Mutex B!");
Mutex mutex = Mutex.OpenExisting("DemoMutex");

for (int i = 0; i < 10; i++)
{
    if (mutex.WaitOne())
    {
       Console.WriteLine($"Mutex B {i} ");
       mutex.ReleaseMutex();
    }
}
