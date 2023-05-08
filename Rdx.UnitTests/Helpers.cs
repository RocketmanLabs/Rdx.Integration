using Rdx.ActionInterface;
using Rdx.Core;
using Rdx.DataConnection;

namespace Rdx.UnitTests;

public static class Helpers
{
    public class DummyAction : _RdxAction
    {
        public DummyAction(string key) : base(key, new Args()) { }
    }

    public static ActionCatalog LoadCatalogWithExampleActions()
    {
        ActionCatalog cat = new ActionCatalog();
        cat.Add("CreateTreasure", new DummyAction("CreateAction"));
        cat.Add("FetchTreasure", new DummyAction("FetchTreasure"));
        cat.Add("UpdateTreasure", new DummyAction("UpdateTreasure"));
        cat.Add("DeleteTreasure", new DummyAction("DeleteTreasure"));
        return cat;
    }
}
