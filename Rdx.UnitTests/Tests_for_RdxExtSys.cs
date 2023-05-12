using Rdx.ActionInterface;
using Rdx.Core;

namespace Rdx.UnitTests;

public class Tests_for_RdxExtSys
{
    [Fact]
    public void RdxExtSys_SUCCESS_matching_system_definitions()
    {
        SystemDefinition sysDefRd = Helpers.MakeRdSysDef("Results Direct Emulator", "EMU", "1.0.0.0");
        SystemDefinition sysDefCs = Helpers.MakeCsSysDef("Results Direct Simulator", "SIM", "1.2.3.4");

        var ut = new UtRdxExtSys(sysDefRd, sysDefCs);
        Assert.NotNull(ut);
        Assert.True(ut.Match(sysDefRd));
    }

    public class UtRdxExtSys : _RdxExtSys
    {
        public UtRdxExtSys(SystemDefinition rd, SystemDefinition cs) : base(rd, cs) { }
    }
}
