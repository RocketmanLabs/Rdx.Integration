using Rdx.Core;

namespace Rdx.UnitTests;
public class Tests_for_SystemDefinition
{
    [Fact]
    public void SysDef_SUCCESS_Create_and_identify_Empty()
    {
        var sysdef = SystemDefinition.Empty;
        Assert.True(sysdef.IsEmpty);
    }

    [Fact]
    public void SysDef_SUCCESS_Generate_guid_ID()
    {
        // if Id is passed in, it will be used - if null is passed in, a random Guid is generated
        var sysdef = new SystemDefinition(null);
        Assert.NotNull(sysdef);
        Assert.False(string.IsNullOrEmpty(sysdef.Id));

        var id = Guid.NewGuid().ToString();
        var sysdef2 = new SystemDefinition(id);
        Assert.NotNull(sysdef2);
        Assert.Equal(id, sysdef2.Id);
    }

    [Fact]
    public void SysDef_SUCCESS_creating_RD_Product()
    {
        var id = Guid.NewGuid().ToString();
        var sysdef = SystemDefinitionFactory.BuildRdProduct(id, "ENG", "Engagefully", "4.3.2.111", isMock: false);
        Assert.NotNull(sysdef);
        Assert.Equal(id, sysdef.Id);

        Assert.Equal("ENG", sysdef.Key);
        Assert.Equal("Engagefully", sysdef.ProductName);
        Assert.Equal("4.3.2.111", sysdef.Version);
        Assert.Equal(ExternalSystemCode.RD, sysdef.ExternalType);
    }

    [Fact]
    public void SysDef_SUCCESS_creating_CS()
    {
        var id = Guid.NewGuid().ToString();
        var sysdef = SystemDefinitionFactory.BuildCustomerSystem(id, "IMP", "iMIS Pro", "100.3.0", isMock: false);
       
        Assert.Equal(ExternalSystemCode.CS, sysdef.ExternalType);
    }

    [Fact]
    public void SysDef_SUCCESS_creating_Mock_RD()
    {
        var id = Guid.NewGuid().ToString();
        var sysdef = SystemDefinitionFactory.BuildCustomerSystem(id, "ENG", "Engagefully", "4.3.2.111", isMock: true);

        Assert.Equal(ExternalSystemCode.RD | ExternalSystemCode.MOCK, sysdef.ExternalType);
    }

    [Fact]
    public void SysDef_SUCCESS_creating_Mock_CS()
    {
        var id = Guid.NewGuid().ToString();
        var sysdef = SystemDefinitionFactory.BuildCustomerSystem(id, "IMP", "iMIS Pro", "100.3.0", isMock: true);

        Assert.Equal(ExternalSystemCode.CS | ExternalSystemCode.MOCK, sysdef.ExternalType);

    }
}