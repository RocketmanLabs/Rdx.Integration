using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdx.Startup.ConsoleExecutable
{
    internal static class CC
    {
        public static void Write(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(msg);
            Console.ResetColor();
        }

        public static void WriteLine(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void Stripe(char c = '=', ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            var width = Console.WindowWidth;
            var stripe = new string(c, width);
            WriteLine(stripe, fg, bg);
        }

        public static void Center(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            var width = (Console.WindowWidth - msg.Length) / 2;
            var pad = new string(' ', width);
            Write(pad);
            Write(msg);
            WriteLine(pad);
        }
    }
}
