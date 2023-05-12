using System.Reflection.Metadata;

namespace Rdx.Core;
public static class GlobalConstants
{
    public const int ELEMENT_NOT_FOUND = -1;

    public const string CUSTOMER_CONFIG_FILE = "externalsystems.cfg";

    // Values for Rs.HttpStatusCode:

    public const string OK = "200 OK";
    public const string AUTH_FAIL = "401 Unauthorized";                 // authentication failed
    public const string NOTFOUND = "404 Not Found";                     // content not found (SERVER_NOTIMPLEMENTED is for missing Action)
    public const string BADREQUEST = "400 Bad Request";                 // missing arg, malformed or incomplete request
    public const string INTERNAL_ERROR = "500 Internal Server Error";   // unable to match Rq.Endpoint to Action Key
    public const string SERVER_NOTIMPLEMENTED = "501 Not Implemented";  // unable to match Rq.Endpoint to Action Key
    public const string SERVER_GATEWAY = "502 Bad Gateway";             // server unable to provide a response
    public const string SERVER_UNAVAILABLE = "503 Service Unavailable"; // service is alive but unresponsive
    public const string SERVER_TIMEOUT= "504 Timeout";                  // request timed out
}