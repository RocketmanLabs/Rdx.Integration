using Rdx.Core;
using Rdx.Core.Exceptions;
using Rdx.DataConnection;
using System.Text;

namespace Rdx.Startup.ConsoleExecutable;

internal class Program
{
    // power on
    static void Main(string[] args)
    {
        try
        {
            RunHarness harness = new();
            RdxDataConnector? dc = null;
            ExternalSystems? xs = null;

            xs = ExternalSystems.DevTestLoad();
            dc = new RdxDataConnector(xs);

            var fg = ConsoleColor.Yellow;
            var bg = ConsoleColor.DarkBlue;
            CC.Stripe('=', fg, bg);
            CC.Center("Results Direct Extensibility Framework", fg, bg);
            CC.Center("Version: 1.0.0.0", fg, bg);
            CC.Stripe('=', fg, bg);
            CC.WriteLine("");

            CC.SetIndent(6);

            // gather query (endpoint and args) in a loop and send to DC for processing
            // until 'ESC' is pressed

            fg = ConsoleColor.Black;
            bg = ConsoleColor.White;
            CC.Stripe('%', fg, bg);

            fg = ConsoleColor.White;
            bg = ConsoleColor.Black;
            CC.WriteLine("QUERY TEST LOOP:");
            CC.WriteLine("Enter query endpoint?name1=value1&name2=value2...");
            fg = ConsoleColor.Gray;
            CC.WriteLine("Press ESC to quit.");

            if (ConsoleQueryEditor.QueryLoop(harness, xs, dc) != StatusCode.OK) throw new RdxInvalidOperationException($"Query loop terminated: {dc.ErrorMessages}");
        }
        catch (Exception ex)
        {
            CC.Error(ex.Message);
            return;
        }
    }
}

public class RunHarness
{
    public RunHarness() { }

    public Stack<Exception> Errors { get; private set; } = new();
    public bool HasError => Errors.Any();

    public StatusCode Init(ref ExternalSystems cfg, ref RdxDataConnector dc)
    {
        try
        {
            cfg = ExternalSystems.DevTestLoad();
            dc = new(cfg);
            if (dc.Discovery() != StatusCode.OK) throw new RdxInvalidOperationException($"Error starting RdxDataConnector: {dc.ErrorMessages}");
            return StatusCode.OK;
        }
        catch (Exception ex)
        {
            CC.Error(ex);
            return StatusCode.ERROR;
        }
    }

    public StatusCode RunSingle(RdxDataConnector dc, string queryUrl)
    {
        // convert the query to an Rq packet and create a matching response packet
        Rq request = Rq.BuildFromQuery(queryUrl);
        Rs response = new(request);

        CC.WriteLine($"Submitting {request.QueryEndpoint} request to RdxDataConnector...");
        //if (dc.Execute(request, response) != StatusCode.OK)
        //{
        //    return StatusCode.ERROR;
        //}
        return StatusCode.OK;
    }
}
public static class ConsoleQueryEditor
{
    public static StatusCode QueryLoop(RunHarness harness, ExternalSystems xs, RdxDataConnector dc)
    {
        var fg = ConsoleColor.Yellow;
        var bg = ConsoleColor.Black;

        StringBuilder querySb = new();
        bool runLoop = true;
        bool charLoop = true;

        // Queries get executed in this loop
        while (runLoop)
        {
            CC.Write("> ");

            // Query gets built, character by character, in this loop
            while (charLoop)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: false);  // echos key to console
                if (key.Key == ConsoleKey.Escape)
                {
                    charLoop = false;
                    runLoop = false;
                    continue;
                }
                if (key.Key == ConsoleKey.Backspace && querySb.Length > 0)
                {
                    querySb.Remove(querySb.Length - 1, 1);
                    continue;
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    charLoop = false;
                    continue;
                }
                querySb.Append(key.KeyChar);
            }
        }
        return StatusCode.OK;
    }
}