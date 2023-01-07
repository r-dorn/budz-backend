
using System.Net;

namespace budz_backend.Exceptions;

public class InvalidRecordException : Exception
{
    public HttpStatusCode Code { get;  }
    public InvalidRecordException(string message, HttpStatusCode statusCode=HttpStatusCode.NotFound)
        : base(message)
    {
        Code = statusCode;
    }
}