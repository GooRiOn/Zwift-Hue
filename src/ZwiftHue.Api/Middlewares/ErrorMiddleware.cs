using ZwiftHue.Core.Exceptions;

namespace ZwiftHue.Api.Middlewares;

internal sealed class ErrorMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ZwiftHueException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new ErrorResponseModel(ex.Message));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;

        }
    }

    private record ErrorResponseModel(string Message);
}