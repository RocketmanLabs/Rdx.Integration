using Rdx.Core.Interfaces;

namespace Rdx.Core;

public static class SystemDefinitionFactory
{
    public static ISystemDefinition BuildRdProduct(string? existingId, string key, string prodName, string version, bool isMock) =>
        wizardBehindTheCurtain(
            key,
            existingId,
            ExternalSystemCode.RD | (isMock ? ExternalSystemCode.MOCK : ExternalSystemCode.NOTHING),
            prodName,
            version);

    public static ISystemDefinition BuildCustomerSystem(string? existingId, string key, string prodName, string version, bool isMock) =>
        wizardBehindTheCurtain(
            key,
            existingId,
            ExternalSystemCode.CS | (isMock ? ExternalSystemCode.MOCK : ExternalSystemCode.NOTHING),
            prodName,
            version);

    private static ISystemDefinition wizardBehindTheCurtain(string key, string? existingId, ExternalSystemCode xtype, string prodname, string version, bool isMock = false)
    {
        var sysDef = new SystemDefinition(existingId)
        {
            Key = key,
            ExternalType = xtype,
            ProductName = prodname,
            Version = version
        };
        return sysDef;
    }
}

