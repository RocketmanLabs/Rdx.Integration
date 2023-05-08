namespace Rdx.Core.Interfaces
{
    public interface ISystemDefinition
    {
        ExternalSystemCode ExternalType { get; set; }
        string Id { get; }
        bool IsEmpty { get; }
        string Key { get; set; }
        string ProductName { get; set; }
        string Version { get; set; }
    }
}