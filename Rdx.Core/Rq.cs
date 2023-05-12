using Rdx.Core.Interfaces;

namespace Rdx.Core;

public record Rq
{
    public Rq() { }

    /// <summary>
    /// When Rq is used in Rs ctor(), this value is copied to CorrelationId
    /// to provide traceability.
    /// </summary>
    public string Id = Guid.NewGuid().ToString();

    public DateTime Timestamp => DateTime.UtcNow;

    /// <summary>
    /// The end of this string must match key in an installed Action (case insensitive).
    /// </summary>
    public string? RequestorEndpoint { get; set; }
    public string? QueryEndpoint { get; set; }

    /// <summary>
    /// Required arguments to be sent to CS.
    /// </summary>
    public Args Criteria { get; set; } = new();

    /// <summary>
    /// Deconstructs a query into a request packet.  Use this for queries
    /// expressed as a URL.  The query string is optional.
    /// The caller should test for QueryEndpoint to be not null to see if
    /// all went well.
    /// 
    /// An Arg can be present with a null Value and have ValueSet == false.
    /// This means it expects to get its value from a default value
    /// supplier such as ExpectedArgs.  This happens during Arg validation
    /// in _RdxAction.
    /// </summary>
    public static Rq BuildFromQuery(string queryUrl)
    {
        Rq rq = new();

        string[] parts = queryUrl.Split("?", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);   // foobar.htm, x=1&y=2&z
        string url = parts[0].Replace("/", @"\");
        string[] paths = url.Split(@"\");
        rq.QueryEndpoint = paths[paths.Length - 1];

        rq.Criteria = new Args();
        if (parts.Length > 1)                   // if we have a query string...
        {
            string[] nvp = parts[1].Split("&", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries); // x=1, y=2, z
            foreach (string pair in nvp)
            {
                string[] nvpParts = pair.Split("=", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);    // x, 1 and y, 2 and z
                int len = nvpParts.Length;
                Arg arg = new Arg(nvpParts[0], len > 1 ? nvpParts[1] : null, null); // creates an Arg for each one
                rq.Criteria.Add(arg);
            }
        } 
        return rq;
    }
}
