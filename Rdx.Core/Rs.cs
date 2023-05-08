namespace Rdx.Core;

public class Rs
{
    public Rs() { }

    public Rs(Rq request)
    {
        CorrelationId = request.Id;
    }

    /// <summary>
    /// Set to Rq.Id in ctor().  Provides traceability.
    /// </summary>
    public string? CorrelationId { get; private set; }

    public string? Message { get; set; }


    public string HttpStatusCode { get; set; } = GlobalConstants.OK;
    public string? RawResponse { get; set; }
    public string? ReturnValue { get; set; }

    public Stack<Exception> Errors = new();
    public bool HasError => Errors.Any();
}
