namespace Rdx.Startup.ConsoleExecutable;

public static class CC
{
    static int Indent = 0;

    public static void Error(Exception ex)=> CC.Error($"[{ex.GetType().Name}]: {ex.Message}");

    public static void Error(string msg)
    {
        CC.Right(msg, ConsoleColor.White, ConsoleColor.DarkRed);
    }

    public static void SetIndent(int numSpaces = 0)
    {
        Indent = numSpaces;
    }

    public static void Write(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
        var pos = Console.GetCursorPosition();
        if (pos.Left == 0 && Indent > 0)
        {
            Console.Write(new String(' ', Indent));
        }
        Console.Write(msg);
        Console.ResetColor();
    }

    public static void WriteLine(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        CC.Write(msg);
        Console.WriteLine("");
    }

    public static void Stripe(char c = '=', ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        var xIndent = Indent;
        Indent = 0;

        var width = Console.WindowWidth;
        var stripe = new string(c, width);
        WriteLine(stripe, fg, bg);

        Indent = xIndent;
    }

    public static void Right(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        var xIndent = Indent;
        Indent = 0;

        var width = Console.WindowWidth - msg.Length;
        var pad = new string(' ', width);
        Write(pad, fg, bg);
        WriteLine(msg, fg, bg);

        Indent = xIndent;
    }

    public static void Center(string msg, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        var xIndent = Indent;
        Indent = 0;

        var width = (Console.WindowWidth - msg.Length) / 2;
        var pad = new string(' ', width);
        Write(pad, fg, bg);
        Write(msg, fg, bg);
        WriteLine(pad, fg, bg);

        Indent = xIndent;
    }
}
