using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.commons;

namespace net.console.run
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleEx.BasicFont = 5;
            ConsoleEx.BeginBufferMode();
            ConsoleEx.Write("Hallo \x1B[31;42;1m\x1B[3B\x1B[10CWelt");
            ConsoleEx.EndBufferMode();
            ConsoleEx.ResetColor();

            ConsoleEx.ReadLine();
        }
    }
}
