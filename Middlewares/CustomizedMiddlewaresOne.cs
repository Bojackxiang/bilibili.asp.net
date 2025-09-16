public class CustomizedMiddlewaresOne : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    await context.Response.WriteAsync(" customized class name - 1");
    await next(context);
  }
}
