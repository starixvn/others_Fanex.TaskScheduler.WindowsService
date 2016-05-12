using System;
using System.IO;

namespace SimpleConsole
{
    public class Program
    {
        public static int Main()
        {
            Console.Write("Exit with code: 0");
            File.WriteAllText("output.txt", $"Run at: {DateTime.Now}");

            return 0;
        }
    }
}
