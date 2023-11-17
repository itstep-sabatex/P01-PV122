// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int a = 0;

Object locked=new object();
void Increment(string threadName)
{
    lock (locked)
    {
        a++;
        //Console.WriteLine($"{threadName} a={a}");
    }
    
}
void IncrementWithMonitor(string threadName)
{
    Monitor.Enter(locked);
    {

    }

    if (Monitor.TryEnter(locked, 1500))
    {
        a++;
        Thread.Sleep(1000);
        Console.WriteLine($"{threadName} a={a}");
    }
    else
    {
        Console.WriteLine($"{threadName} a={a} not enter to critical section!");
    }

}
//Task.Run(() =>
//{
//    for (int i = 0; i < 10; i++)
//    {
//            Increment("Task1");
//    }
//});

//Task.Run(() =>
//{
//    for (int i = 0; i < 10; i++)
//    {
//            Increment("Task2");

//    }
//});
Task.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
        IncrementWithMonitor("Task1");
    }
});

Task.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
        IncrementWithMonitor("Task2");

    }
});
Thread.Sleep(10000);