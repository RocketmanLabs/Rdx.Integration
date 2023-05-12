using Rdx.ActionInterface;
using Rdx.Core;
using Rdx.DataConnection;

namespace Rdx.UnitTests;

public static class Helpers
{
    public static ActionCatalog LoadCatalogWithExampleActions()
    {
        ActionCatalog cat = new ActionCatalog();
        cat.Add("CreateTreasure", new DummyAction("CreateTreasure"));
        cat.Add("FetchTreasure", new DummyAction("FetchTreasure"));
        cat.Add("UpdateTreasure", new DummyAction("UpdateTreasure"));
        cat.Add("DeleteTreasure", new DummyAction("DeleteTreasure"));
        return cat;
    }
    public class DummyAction : _RdxAction
    {
        public DummyAction(string key) : base(key, new Args()) { }
    }


    public const string RDPROD_KEY = "ENG";
    public const string RDPROD_NAME = "Engagefully";
    public const string RDPROD_VERSION = "2-5-0";
    public const string CS1_KEY = "IMP";
    public const string CS1_PRODUCT_NAME = "iMIS Professional";
    public const string CS1_VERSION = "100-3-0";
    public const string CS2_KEY = "PLS";
    public const string CS2_PRODUCT_NAME = "Planstone";
    public const string CS2_VERSION = "6-4-280";

    public static ExternalSystems MakeXSys(string orgName, string fullPath)
    {
        var sysDefRd = Helpers.MakeRdSysDef(RDPROD_NAME, RDPROD_KEY, RDPROD_VERSION);
        var sysDefCs1 = Helpers.MakeCsSysDef(CS1_PRODUCT_NAME, CS1_KEY, CS1_VERSION);
        var sysDefCs2 = Helpers.MakeCsSysDef(CS2_PRODUCT_NAME, CS2_KEY, CS2_VERSION);
        var xsys = new ExternalSystems() { CustomerOrganizationName = orgName, FullPath = fullPath };
        xsys.Add(sysDefRd);
        xsys.Add(sysDefCs1);
        xsys.Add(sysDefCs2);
        return xsys;
    }

    public static SystemDefinition MakeRdSysDef(string name, string key, string version) => new SystemDefinition() { 
        ProductName = name, 
        Key = key, 
        Version = version, 
        ExternalType = ExternalSystemCode.RD | ExternalSystemCode.MOCK 
    };


    public static SystemDefinition MakeCsSysDef(string name, string key, string version) => new SystemDefinition()
    {
        ProductName = name,
        Key = key,
        Version = version,
        ExternalType = ExternalSystemCode.CS | ExternalSystemCode.MOCK
    };

    public static void DeleteTestFile(string fullPath)
    {
        File.Delete(fullPath);
    }
}
