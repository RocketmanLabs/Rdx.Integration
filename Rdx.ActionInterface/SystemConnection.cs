using Rdx.ActionInterface.Interfaces;
using Rdx.Core;
using Rdx.Core.Interfaces;
using System.Net;

namespace Rdx.ActionInterface;

public record SystemConnection : SystemDefinition, ISystemConnection
{
    private Uri Url;

    public SystemConnection(SystemDefinition sysdef, string baseAddress, ICredentials creds, SocketsHttpHandler handler)
    {
        Id = sysdef.Id;
        Key = sysdef.Key;
        ProductName = sysdef.ProductName;
        Version = sysdef.Version;
        ExternalType = sysdef.ExternalType;
        Url = new Uri(baseAddress);
        Link = new(handler);
    }
    /*
     * SystemDefinition:
     *   string Id;
     *   string Key;
     *   string ProductName;
     *   RdVersion Version;
     *   ExternalSystemCode ExternalType;
     *   bool IsEmpty;
     */

    public StatusCode Authenticate(ICredentials creds, string endpoint, bool requestHeader = false)
    {
        // TODO: HttpClient authentication, set IsAuthenticated if successful, or error if not
        throw new NotImplementedException();
    }

    public HttpClient Link { get; private set; } = new();
    public bool IsAuthenticated { get; private set; }

    public Exception? Error { get; private set; }
    public bool HasError => Error is not null;
}
