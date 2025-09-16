
public static class CustomizedMiddlewareExtension
{
  // this app, 相当于 下面的方法扩展 app
  public static IApplicationBuilder UseCustomizedMiddlewaresOneExtension(this IApplicationBuilder app)
  {
    return app.UseMiddleware<CustomizedMiddlewaresOne>();
  }

  // public static IApplicationBuilder DoSomething(this IApplicationBuilder app)
  // {
  //   return app.Use(async (context, next) =>
  //       {
  //         Console.WriteLine("Do something for every request");
  //         await next();
  //       });
  // }
}
