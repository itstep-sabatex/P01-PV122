// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int a = 0;

Object locked=new object();
void Increment(string threadName)
{
    lock (locked)
    {
        a++;
        Console.WriteLine($"{threadName} a={a}");
    }
    
}

Task.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
            Increment("Task1");
    }
});

Task.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
            Increment("Task2");
       
    }
});

Thread.Sleep(5000);