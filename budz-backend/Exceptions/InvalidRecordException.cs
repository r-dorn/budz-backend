using System.Net;

namespace budz_backend.Exceptions;

public class InvalidRecordException : Exception
{
    public InvalidRecordException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        : base(message)
    {
        Code = statusCode;
    }

    public HttpStatusCode Code { get; }
}