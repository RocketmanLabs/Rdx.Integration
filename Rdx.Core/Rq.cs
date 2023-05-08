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
    public string? Endpoint { get; set; }

    /// <summary>
    /// Required arguments to be sent to CS.
    /// </summary>
    public Args Criteria { get; set; } = new();
}
