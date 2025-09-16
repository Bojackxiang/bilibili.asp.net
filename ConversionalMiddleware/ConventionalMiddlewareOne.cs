// example of conventional middleware

using System.Reflection.Metadata.Ecma335;

namespace FirstProject.ConventionalMiddleware
{
  // middleware class  的定义
  public class ConventionalMiddlewareOne
  {
    private readonly RequestDelegate _next;

    public ConventionalMiddlewareOne(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext http)
    {
      // todo: adding the business logics
      bool validatedFirstName = http.Request.Query.ContainsKey("firstName");
      bool validatedLastName = http.Request.Query.ContainsKey("lastName");
      if (validatedFirstName)
      {
        string fullName = http.Request.Query["firstName"] + " " + http.Request.Query["lastName"];
        await http.Response.WriteAsync(fullName);
      }
      await _next(http);
    }

  }

  // 使用 middleware 然后 加到 extension 上
  public static class ConventionalMiddlewareOneExtension
  {
    public static IApplicationBuilder UseConventionalMiddlewareOne(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ConventionalMiddlewareOne>();
    }
  }
}
