using System;
using System.Threading;

namespace ThreadDemo
{
    internal class Program
    {
        static double MultiplreOneElement(int dim, int i, int j, double[,] a, double[,] b)
        {
            double result = 0;
            //var xDim = a.GetLength(0);
            //var yDim = a.GetLength(1);

            for (int mi = 0; mi < dim; mi++)
            {
                result = result + a[i, mi] * b[mi, j];
            }
            return result;
        }

        static void ThreadA(object p)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"Starting thred {p}");
        }

        static void Main(string[] args)
        {
           
            

            
            for(int i = 0; i < 100; i++)
            {
                int a = i;
                //Console.WriteLine($"Start Thred {i}!");
                var tr = new Thread(ThreadA);
                //tr.IsBackground=true;
                tr.Start($"Start Thred {i}!");
            }
            Thread.Sleep(6000);
            Console.WriteLine("Program ended");
        }
    }
}