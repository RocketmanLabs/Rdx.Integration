namespace Rdx.Core;

public enum StatusCode : short
{
    COMPARES_AS_LESS = -1,
    OK = 0,
    COMPARES_AS_MORE = 1,
    ERROR,
    NOT_FOUND,
    DUPLICATE,
    NULL,
    VALIDATION_ERROR
}
