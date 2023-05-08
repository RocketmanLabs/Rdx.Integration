namespace Rdx.Startup.ConsoleExecutable;

internal class Program
{
    static void Main(string[] args)
    {
        var fg = ConsoleColor.Yellow;
        var bg = ConsoleColor.DarkBlue;
        CC.Stripe('=', fg, bg);
        CC.Center("Results Direct Extensibility Framework", fg, bg);
        CC.Center("Version: 0.0.0.0", fg, bg);
        CC.Stripe('=', fg, bg);
        CC.WriteLine("Loading customer configuration file: ");
    }
}