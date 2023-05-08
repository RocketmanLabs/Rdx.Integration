using Rdx.Core;

namespace Rdx.DataConnection;

public class RdxDataConnector
{
    private ExternalSystems _sysDefs;
    private AdapterCatalog _adapters = new();

    public RdxDataConnector(ExternalSystems sysDefs)
    {
        _sysDefs = sysDefs;
    }

    public Stack<Exception> Errors = new();
    public bool HasError => Errors.Any();

    public static ExternalSystems Discovery(string externalSystemsFilename, AdapterCatalog newAdpCat)
    {
        // TODO: Read file into a new ExternalSystems instance
        // TODO: Scan DLLs, creating an AdapterFile for each
        // TODO: Test AdapterFiles against the ExternalSystems to see if they match
        // TODO: Load the DLLs 
        // TODO: Create the AdapterCatalog

        // TODO: Create the ActionCatalog
        // TODO: Create HttpClient instances for each system
        // TODO: sign in using the endpoint and credentials
        throw new NotImplementedException();
    }
}