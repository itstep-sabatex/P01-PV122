using System.Runtime.InteropServices;
using System.Diagnostics;
namespace DLLImportDemo
{ 
   
    internal static class Program
    { 
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void MessageBox(IntPtr hWnd, string text, string caption, uint uType = 0x02);
        static void Main(string[] args)
        {
            //  eax - l
            //  ecx - h
            //  edx - l -b
            //  ebx  -h - b
            //  add eax,edx 67 ff
            //  addc ecx,ebx
            //  c - 

            Console.WriteLine("Hello, World!");
            IntPtr intPtr = Process.GetCurrentProcess().MainWindowHandle;
            MessageBox(intPtr, "Title dll window", "Demo");
            Thread.Sleep(5000);
            

            // ClassName;Name;
            // Dog;Bobik;5;Gav;brown
            // Kat;Alf;6;May;red

        }
    }
}