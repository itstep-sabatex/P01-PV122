using System.Diagnostics;

namespace ProcessDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = $"/C tar -czvf Archive.tar.gz  C:/tmp/Archive";
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender,args)=>Console.WriteLine($"Error:{args.Data}");


                process.Start();
                process.WaitForExit();
                var result = process.ExitCode;
                Console.WriteLine($"Exit code:{result}");
            }
            Console.ReadKey();

        }
    }
}