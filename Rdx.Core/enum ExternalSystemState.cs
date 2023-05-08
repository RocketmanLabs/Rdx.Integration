namespace Rdx.Core;

public enum ExternalSystemState : byte
{
    UNKNOWN = 0,
    FILE_DISCOVERED,
    ADAPTERS_CATALOGED,
    ACTIONS_CATALOGED,
    SIGNED_IN,
    ACTIVE
}
