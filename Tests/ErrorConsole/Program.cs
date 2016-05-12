using System;
using System.IO;

namespace ErrorConsole
{
    public class Program
    {
        public static int Main()
        {
            Console.Write("Exit with code: 1");
            File.WriteAllText("output.txt", $"Run at: {DateTime.Now}");

            return 1;
        }
    }
}
