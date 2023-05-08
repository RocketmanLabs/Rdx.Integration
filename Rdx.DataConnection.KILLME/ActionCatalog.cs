using Rdx.ActionInterface;

namespace Rdx.DataConnection;

public class ActionCatalog : Dictionary<string, _RdxAction>
{
    public _RdxAction? Find(string? queryEndpoint) =>
        this.Values.FirstOrDefault(x => queryEndpoint is not null 
            && queryEndpoint.EndsWith(
                x.Key, 
                StringComparison.InvariantCultureIgnoreCase)
        );
}
