# Adapter - Demonstrator Greetings RDEMU()=RDSIM()
Demonstrates Actions that do not require a CS to operate. The "UT*" Actions support Unit Testing.  The "Hello"
and "Goodbye" Actions show how to create an Action with and without arguments.

## Hello(username) 
When the "Hello" query is detected from the ENG product, this action returns a greeting in response.ReturnValue 
that incorporates the username argument.

## Goodbye()
When the "Goodbye" query is detected from the ENG product, this action returns 
"Goodbye!!" in response.ReturnValue to show how to construct an Action with no arguments.

## UTNoAction()
When this query is detected from the ENG product or the mocking emulator, it will find 
there is no Action supporting it, and throws an error.

## UTMissingArg(x, y, z)
This query gets called without the Z argument to test error handling.  It returns the arguments
received once the error is cleared with the Override discussed below.

# Using the Override Rdx.Adapters.UT.Override
Note: The RDEMU()=RDSIM()UnitTest.dll is an Override that cures the missing argument in UTMissingArg(x, y, z).
Including it in the RdxDataConnector stack is part of Unit Testing.
