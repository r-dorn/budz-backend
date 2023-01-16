namespace budz_backend.Middleware;

using budz_backend.Exceptions;

public class InvalidRecordHandler
{
    private readonly RequestDelegate _next;


    public InvalidRecordHandler(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidRecordException e)
        {
            context.Response.StatusCode = (int)e.Code;
            await context.Response.WriteAsync(e.Message);
        }
    }

}